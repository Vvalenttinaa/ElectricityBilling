using ElectricityBilling.Application.DTOs;
using ElectricityBilling.Application.Exceptions;
using ElectricityBilling.Application.Interfaces;
using ElectricityBilling.Domain.Entities;

namespace ElectricityBilling.Application.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _planRepository;

        public PlanService(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<List<PlanDto>> GetAllAsync()
        {
            var plans = await _planRepository.GetAllAsync();

            return plans.Select(plan => new PlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Discount = plan.Discount,
                IsActive = plan.IsActive,

                PricingTiers = plan.PricingTiers
                    .OrderBy(t => t.SortOrder)
                    .Select(t => new PricingTierDto
                    {
                        Id = t.Id,
                        Threshold = t.Threshold,
                        PricePerKwh = t.PricePerKwh,
                        SortOrder = t.SortOrder
                    })
                    .ToList()
            }).ToList();
        }
        public async Task<PlanDto> CreateAsync(CreatePlanDto dto)
        {
           
            var plan = new Plan
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Description = dto.Description,
                Discount = dto.Discount,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,

                PricingTiers = dto.PricingTiers
                    .Select(tier => new PricingTier
                    {
                        Id = Guid.NewGuid(),
                        Threshold = tier.Threshold,
                        PricePerKwh = tier.PricePerKwh,
                        SortOrder = tier.SortOrder
                    })
                    .ToList()
            };

            var createdPlan = await _planRepository.AddAsync(plan);

            return new PlanDto
            {
                Id = createdPlan.Id,
                Name = createdPlan.Name,
                Description = createdPlan.Description,
                Discount = createdPlan.Discount,
                IsActive = createdPlan.IsActive,

                PricingTiers = createdPlan.PricingTiers
                    .OrderBy(t => t.SortOrder)
                    .Select(t => new PricingTierDto
                    {
                        Id = t.Id,
                        Threshold = t.Threshold,
                        PricePerKwh = t.PricePerKwh,
                        SortOrder = t.SortOrder
                    })
                    .ToList()
            };
        }
        public async Task<PlanDto> UpdateAsync(
    Guid id,
    UpdatePlanDto dto)
        {
            var plan = await _planRepository.GetByIdForUpdateAsync(id);

            if (plan is null)
                throw new NotFoundException("Plan not found."); ;

            plan.Name = dto.Name.Trim();
            plan.Description = dto.Description;
            plan.Discount = dto.Discount;
            plan.IsActive = dto.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;

            var oldTiers = plan.PricingTiers.ToList();

            _planRepository.RemovePricingTiers(oldTiers);

            plan.PricingTiers.Clear();

            foreach (var tierDto in dto.PricingTiers)
            {
                plan.PricingTiers.Add(
                    new PricingTier
                    {
                        PlanId = plan.Id,
                        Threshold = tierDto.Threshold,
                        PricePerKwh = tierDto.PricePerKwh,
                        SortOrder = tierDto.SortOrder
                    });
            }

            await _planRepository.SaveChangesAsync();

            return new PlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Discount = plan.Discount,
                IsActive = plan.IsActive,

                PricingTiers = plan.PricingTiers
                    .OrderBy(t => t.SortOrder)
                    .Select(t => new PricingTierDto
                    {
                        Id = t.Id,
                        Threshold = t.Threshold,
                        PricePerKwh = t.PricePerKwh,
                        SortOrder = t.SortOrder
                    })
                    .ToList()
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var plan = await _planRepository.GetByIdForUpdateAsync(id);

            if (plan is null)
                return false;

            plan.IsActive = false;
            plan.UpdatedAt = DateTime.UtcNow;

            await _planRepository.SaveChangesAsync();

            return true;
        }


        public async Task<PlanDto> GetByIdAsync(Guid id)
        {
            var plan = await _planRepository.GetByIdAsync(id);

            if (plan is null)
                throw new NotFoundException("Plan not found.");

            return new PlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Discount = plan.Discount,
                IsActive = plan.IsActive,

                PricingTiers = plan.PricingTiers
                    .OrderBy(t => t.SortOrder)
                    .Select(t => new PricingTierDto
                    {
                        Id = t.Id,
                        Threshold = t.Threshold,
                        PricePerKwh = t.PricePerKwh,
                        SortOrder = t.SortOrder
                    })
                    .ToList()
            };
        }

    }
}
