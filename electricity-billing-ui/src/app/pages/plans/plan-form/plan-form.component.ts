import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { PlanService } from '../../../services/plan.service';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-plan-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    MatDividerModule,
    TranslatePipe
  ],
  templateUrl: './plan-form.component.html',
  styleUrl: './plan-form.component.scss',
})
export class PlanFormComponent implements OnInit {
  planForm!: FormGroup;

  isEditMode = false;
  planId?: string;

  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private planService: PlanService,
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.planForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      discount: [
        0,
        [Validators.required, Validators.min(0), Validators.max(100)],
      ],
      isActive: [true],

      pricingTiers: this.fb.array([]),
    });

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.isEditMode = true;
      this.planId = id;

      this.loadPlan(id);
    } else {
      this.addPricingTier();
    }
  }

  get pricingTiers(): FormArray {
    return this.planForm.get('pricingTiers') as FormArray;
  }

  createPricingTier(): FormGroup {
    return this.fb.group({
      id: [null],
      threshold: [null],
      pricePerKwh: [0, [Validators.required, Validators.min(0)]],
      sortOrder: [
        this.pricingTiers.length + 1,
        [Validators.required, Validators.min(1)],
      ],
    });
  }

  addPricingTier(): void {
    this.pricingTiers.push(this.createPricingTier());
  }

  removePricingTier(index: number): void {
    if (this.pricingTiers.length <= 1) {
      return;
    }

    this.pricingTiers.removeAt(index);
  }

  loadPlan(id: string): void {
    this.loading = true;

    this.planService.getById(id).subscribe({
      next: (plan) => {
        this.planForm.patchValue({
          name: plan.name,
          description: plan.description,
          discount: plan.discount,
          isActive: plan.isActive,
        });

        this.pricingTiers.clear();

        plan.pricingTiers.forEach((tier) => {
          this.pricingTiers.push(
            this.fb.group({
              id: [tier.id],
              threshold: [tier.threshold],
              pricePerKwh: [
                tier.pricePerKwh,
                [Validators.required, Validators.min(0)],
              ],
              sortOrder: [
                tier.sortOrder,
                [Validators.required, Validators.min(1)],
              ],
            }),
          );
        });

        this.loading = false;
      },

      error: (error) => {
        console.error(error);

        this.errorMessage = 'Greška pri učitavanju plana.';

        this.loading = false;
      },
    });
  }

  save(): void {
    if (this.planForm.invalid) {
      this.planForm.markAllAsTouched();

      return;
    }

    this.loading = true;
    this.errorMessage = '';

    const plan = this.planForm.value;

    if (this.isEditMode && this.planId) {
      this.planService.update(this.planId, plan).subscribe({
        next: () => {
          this.router.navigate(['/plans']);
        },

        error: (error) => {
          console.error(error);

          this.errorMessage = 'Greška pri izmjeni plana.';

          this.loading = false;
        },
      });
    } else {
      this.planService.create(plan).subscribe({
        next: () => {
          this.router.navigate(['/plans']);
        },

        error: (error) => {
          console.error(error);

          this.errorMessage = 'Greška pri kreiranju plana.';

          this.loading = false;
        },
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/plans']);
  }
}
