export interface PlanCost {
  planId: string;
  planName: string;
  pricePerKwh: number;
  baseCost: number;
  discountAmount: number;
  vatAmount: number;
  ecoTaxAmount: number;
  totalCost: number;
}
