using ElectricityBilling.Application.DTOs;
using ElectricityBilling.Application.Interfaces;
using ElectricityBilling.Domain.Entities;

namespace ElectricityBilling.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IPlanRepository _planRepository;
        private readonly ITaxGroupRepository _taxGroupRepository;
        private readonly IRecommendationLogRepository _recommendationLogRepository;
        private readonly IEmailService _emailService;

        public RecommendationService(
            IPlanRepository planRepository,
            ITaxGroupRepository taxGroupRepository,
            IRecommendationLogRepository recommendationLogRepository,
            IEmailService emailService)
        {
            _planRepository = planRepository;
            _taxGroupRepository = taxGroupRepository;
            _recommendationLogRepository = recommendationLogRepository;
            _emailService = emailService;
        }

        public async Task<RecommendationResponseDto> GetRecommendationAsync(
            RecommendationRequestDto request)
        {
            if (request.MonthlyConsumption <= 0)
            {
                throw new ArgumentException(
                    "Monthly consumption must be greater than zero.");
            }

            var taxGroup =
                await _taxGroupRepository.GetByIdWithActiveTaxRateAsync(
                    request.TaxGroupId);

            if (taxGroup is null)
            {
                throw new ArgumentException(
                    "Tax group not found.");
            }

            var taxRate = taxGroup.TaxRates
                .FirstOrDefault();

            if (taxRate is null)
            {
                throw new InvalidOperationException(
                    "No active tax rate found for the selected tax group.");
            }

            var plans =
                await _planRepository
                    .GetActivePlansWithPricingTiersAsync();

            if (plans.Count == 0)
            {
                throw new InvalidOperationException(
                    "No active plans are available.");
            }

            var calculatedPlans = new List<PlanCostDto>();

            foreach (var plan in plans)
            {
                var pricingTier = GetPricingTier(
                    plan,
                    request.MonthlyConsumption);

                var baseCost =
                    request.MonthlyConsumption
                    * pricingTier.PricePerKwh;

                var discountAmount =
                    baseCost * plan.Discount;

                var costAfterDiscount =
                    baseCost - discountAmount;

                var vatAmount =
                    costAfterDiscount * taxRate.Vat;

                var ecoTaxAmount =
                    costAfterDiscount * taxRate.EcoTax;

                var totalCost =
                    costAfterDiscount
                    + vatAmount
                    + ecoTaxAmount;

                calculatedPlans.Add(new PlanCostDto
                {
                    PlanId = plan.Id,
                    PlanName = plan.Name,
                    PricePerKwh = pricingTier.PricePerKwh,

                    BaseCost = Math.Round(baseCost, 2),
                    DiscountAmount = Math.Round(discountAmount, 2),
                    VatAmount = Math.Round(vatAmount, 2),
                    EcoTaxAmount = Math.Round(ecoTaxAmount, 2),
                    TotalCost = Math.Round(totalCost, 2)
                });
            }

            var orderedPlans = calculatedPlans
                .OrderBy(p => p.TotalCost)
                .ToList();

            var recommendedPlan = orderedPlans.First();

            // ---------------------------------------------
            // LOG RECOMMENDATION
            // ---------------------------------------------

            var recommendationLog = new RecommendationLog
            {
                Id = Guid.NewGuid(),

                ConsumptionKwh =
                    request.MonthlyConsumption,

                TaxGroupId =
                    request.TaxGroupId,

                RecommendedPlanId =
                    recommendedPlan.PlanId,

                EstimatedCost =
                    recommendedPlan.TotalCost,

                CreatedAt =
                    DateTime.UtcNow
            };

            await _recommendationLogRepository
                .AddAsync(recommendationLog);

            // ---------------------------------------------
            // RESPONSE
            // ---------------------------------------------

            if (request.SendEmail)
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    throw new ArgumentException(
                        "Email is required when email delivery is selected.");
                }

                await _emailService.SendRecommendationAsync(
                    request.Email,
                    recommendedPlan.PlanName,
                    request.MonthlyConsumption,
                    recommendedPlan.TotalCost);
            }

            return new RecommendationResponseDto
            {
                RecommendedPlan = recommendedPlan,
                AllPlans = orderedPlans
            };
        }

        private static PricingTier GetPricingTier(
            Plan plan,
            decimal consumption)
        {
            var tier = plan.PricingTiers
                .OrderBy(t => t.SortOrder)
                .FirstOrDefault(t =>
                    t.Threshold == null ||
                    consumption <= t.Threshold);

            if (tier is null)
            {
                throw new InvalidOperationException(
                    $"No pricing tier found for plan {plan.Name}.");
            }

            return tier;
        }
    }
}