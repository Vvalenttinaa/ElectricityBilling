import { PricingTier } from './pricing-tier.model';

export interface Plan {
  id: string;
  name: string;
  description: string;
  discount: number;
  isActive: boolean;
  pricingTiers: PricingTier[];
}
