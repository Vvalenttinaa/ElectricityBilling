import { PlanCost } from './plan-cost.model';

export interface RecommendationResponse {
  recommendedPlan: PlanCost;
  allPlans: PlanCost[];
}
