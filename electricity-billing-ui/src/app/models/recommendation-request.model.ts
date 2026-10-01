export interface RecommendationRequest {
  monthlyConsumption: number;
  taxGroupId: string;
  sendEmail: boolean;
  email?: string | null;
}
