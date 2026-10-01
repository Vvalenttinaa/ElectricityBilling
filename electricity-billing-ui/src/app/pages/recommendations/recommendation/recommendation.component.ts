import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatDividerModule } from '@angular/material/divider';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { TaxGroup } from '../../../models/tax-group.model';
import { RecommendationResponse } from '../../../models/recommendation-response.model';

import { TaxGroupService } from '../../../services/tax-group.service';
import { RecommendationService } from '../../../services/recommendation.service';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-recommendation',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,

    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    MatTableModule,
    MatDividerModule,
    MatCheckboxModule,
    TranslatePipe
  ],
  templateUrl: './recommendation.component.html',
  styleUrl: './recommendation.component.scss',
})
export class RecommendationComponent implements OnInit {
  recommendationForm!: FormGroup;

  taxGroups: TaxGroup[] = [];
  result?: RecommendationResponse;

  loading = false;
  loadingTaxGroups = true;

  errorMessage = '';
  emailSentMessage = '';

  displayedColumns: string[] = [
    'planName',
    'baseCost',
    'discount',
    'vat',
    'ecoTax',
    'total',
  ];

  constructor(
    private fb: FormBuilder,
    private taxGroupService: TaxGroupService,
    private recommendationService: RecommendationService,
  ) {}

  ngOnInit(): void {
    this.recommendationForm = this.fb.group({
      monthlyConsumption: [null, [Validators.required, Validators.min(0.01)]],

      taxGroupId: ['', Validators.required],

      sendEmail: [false],

      email: [''],
    });

    // Email je obavezan samo ako korisnik
    // želi da dobije preporuku putem emaila.
    this.recommendationForm
      .get('sendEmail')
      ?.valueChanges.subscribe((sendEmail: boolean) => {
        const emailControl = this.recommendationForm.get('email');

        if (sendEmail) {
          emailControl?.setValidators([Validators.required, Validators.email]);
        } else {
          emailControl?.clearValidators();

          emailControl?.setValue('');
        }

        emailControl?.updateValueAndValidity();
      });

    this.loadTaxGroups();
  }

  loadTaxGroups(): void {
    this.loadingTaxGroups = true;

    this.taxGroupService.getAll().subscribe({
      next: (data) => {
        this.taxGroups = data.filter((x) => x.isActive);

        this.loadingTaxGroups = false;
      },

      error: (error) => {
        console.error(error);

        this.errorMessage = 'Greška pri učitavanju poreskih grupa.';

        this.loadingTaxGroups = false;
      },
    });
  }

  calculate(): void {
    if (this.recommendationForm.invalid) {
      this.recommendationForm.markAllAsTouched();

      return;
    }

    this.loading = true;

    this.errorMessage = '';
    this.emailSentMessage = '';

    this.result = undefined;

    const sendEmail = this.recommendationForm.value.sendEmail ?? false;

    const request = {
      monthlyConsumption: Number(
        this.recommendationForm.value.monthlyConsumption,
      ),

      taxGroupId: this.recommendationForm.value.taxGroupId,

      sendEmail: sendEmail,

      email: sendEmail ? this.recommendationForm.value.email : null,
    };

    this.recommendationService.getRecommendation(request).subscribe({
      next: (response) => {
        this.result = response;

        if (sendEmail) {
          this.emailSentMessage =
            'Recommendation has been successfully sent to your email.';
        }

        this.loading = false;
      },

      error: (error) => {
        console.error(error);

        this.errorMessage = 'Greška pri izračunavanju preporuke.';

        this.loading = false;
      },
    });
  }
}
