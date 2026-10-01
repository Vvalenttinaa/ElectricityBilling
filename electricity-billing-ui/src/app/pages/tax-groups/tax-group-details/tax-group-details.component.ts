import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { TaxGroup } from '../../../models/tax-group.model';

import { TaxGroupService } from '../../../services/tax-group.service';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-tax-group-details',
  standalone: true,
  imports: [CommonModule, RouterLink, TranslatePipe],
  templateUrl: './tax-group-details.component.html',
  styleUrl: './tax-group-details.component.scss',
})
export class TaxGroupDetailsComponent implements OnInit {
  taxGroup?: TaxGroup;

  loading = true;

  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taxGroupService: TaxGroupService,
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.errorMessage = 'Tax Group ID nije pronađen.';

      this.loading = false;

      return;
    }

    this.loadTaxGroup(id);
  }

  loadTaxGroup(id: string): void {
    this.loading = true;

    this.taxGroupService.getById(id).subscribe({
      next: (taxGroup) => {
        this.taxGroup = taxGroup;

        this.loading = false;
      },

      error: (error) => {
        console.error(error);

        this.errorMessage = 'Greška pri učitavanju poreske grupe.';

        this.loading = false;
      },
    });
  }

  deleteTaxGroup(): void {
    if (!this.taxGroup) {
      return;
    }

    const confirmed = confirm(
      `Are you sure you want to delete tax group "${this.taxGroup.name}"?`,
    );

    if (!confirmed) {
      return;
    }

    this.taxGroupService.delete(this.taxGroup.id).subscribe({
      next: () => {
        this.router.navigate(['/tax-groups']);
      },

      error: (error) => {
        console.error(error);

        this.errorMessage = 'Greška pri brisanju poreske grupe.';
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/tax-groups']);
  }
}
