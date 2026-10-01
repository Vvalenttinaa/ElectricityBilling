export interface PricingTier {
  id: string;
  threshold: number | null;
  pricePerKwh: number;
  sortOrder: number;
}
