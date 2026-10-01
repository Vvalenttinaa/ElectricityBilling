export interface TaxRate {
  id: string;
  taxGroupId: number;
  vat: number;
  ecoTax: number;
  validFrom: string;
  validTo: string | null;
  isActive: boolean;
}
