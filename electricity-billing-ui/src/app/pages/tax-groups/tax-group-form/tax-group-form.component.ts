import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { ActivatedRoute, Router } from '@angular/router';

import { TaxGroupService } from '../../../services/tax-group.service';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-tax-group-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    TranslatePipe
  ],
  templateUrl: './tax-group-form.component.html',
  styleUrl: './tax-group-form.component.scss',
})
export class TaxGroupFormComponent implements OnInit {
  taxGroupForm!: FormGroup;

  isEditMode = false;

  taxGroupId?: string;

  loading = false;

  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private taxGroupService: TaxGroupService,
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.taxGroupForm = this.fb.group({
      name: ['', Validators.required],

      description: [''],

      vat: [0, [Validators.required, Validators.min(0), Validators.max(1)]],

      ecoTax: [
        0,
        [Validators.required, Validators.min(0), Validators.max(1)],
      ],

      isActive: [true],
    });

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.isEditMode = true;

      this.taxGroupId = id;

      this.loadTaxGroup(id);
    }
  }

  loadTaxGroup(id: string): void {
    this.loading = true;

    this.taxGroupService.getById(id).subscribe({
      next: (taxGroup) => {
        this.taxGroupForm.patchValue({
          name: taxGroup.name,
          description: taxGroup.description,
          vat: taxGroup.vat,
          ecoTax: taxGroup.ecoTax,
          isActive: taxGroup.isActive,
        });

        this.loading = false;
      },

      error: (error) => {
        console.error(error);

        this.errorMessage = 'Greška pri učitavanju poreske grupe.';

        this.loading = false;
      },
    });
  }

  save(): void {
    if (this.taxGroupForm.invalid) {
      this.taxGroupForm.markAllAsTouched();

      return;
    }

    this.loading = true;
    this.errorMessage = '';

    const taxGroup = this.taxGroupForm.value;

    if (this.isEditMode && this.taxGroupId) {
      this.taxGroupService.update(this.taxGroupId, taxGroup).subscribe({
        next: () => {
          this.router.navigate(['/tax-groups']);
        },

        error: (error) => {
          console.error(error);

          this.errorMessage = 'Greška pri izmjeni poreske grupe.';

          this.loading = false;
        },
      });
    } else {
      this.taxGroupService.create(taxGroup).subscribe({
        next: () => {
          this.router.navigate(['/tax-groups']);
        },

        error: (error) => {
          console.error(error);

          this.errorMessage = 'Greška pri kreiranju poreske grupe.';

          this.loading = false;
        },
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/tax-groups']);
  }
}
