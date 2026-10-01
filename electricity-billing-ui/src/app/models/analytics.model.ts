export interface PlanSelectionTrend {
  planId: string;
  planName: string;
  selectionCount: number;
  percentage: number;
}

export interface AnalyticsSummary {
  totalRecommendations: number;
  averageConsumptionKwh: number;
  averageEstimatedCost: number;
  planSelectionTrends: PlanSelectionTrend[];
}
