import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

import { Plan } from '../../../models/plan.model';
import { PlanService } from '../../../services/plan.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { ConfirmDialogComponent } from '../../../shared/confirm-dialog/confirm-dialog.component';
import { TranslatePipe } from '@ngx-translate/core';
@Component({
  selector: 'app-plans-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatButtonModule,
    MatCardModule,
    MatDialogModule,
    TranslatePipe
  ],
  templateUrl: './plans-list.component.html',
  styleUrl: './plans-list.component.scss',
})
export class PlansListComponent implements OnInit {
  plans: Plan[] = [];
  loading = true;
  errorMessage = '';

  displayedColumns: string[] = [
    'name',
    'description',
    'discount',
    'status',
    'pricingTiers',
    'actions',
  ];

  constructor(
    private planService: PlanService,
    private dialog: MatDialog,
  ) {}

  ngOnInit(): void {
    this.loadPlans();
  }

  loadPlans(): void {
    this.loading = true;
    this.errorMessage = '';

    this.planService.getAll().subscribe({
      next: (data) => {
        this.plans = data;
        this.loading = false;
      },

      error: (error) => {
        console.error('Error loading plans:', error);

        this.errorMessage = 'Greška pri učitavanju planova.';

        this.loading = false;
      },
    });
  }

  deletePlan(id: string, name: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '420px',

      data: {
        title: 'Delete Plan',
        message: `Are you sure you want to delete plan "${name}"?`,
        confirmText: 'Delete',
        cancelText: 'Cancel',
      },
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.planService.delete(id).subscribe({
        next: () => {
          this.loadPlans();
        },

        error: (error) => {
          console.error('Error deleting plan:', error);

          this.errorMessage = 'Greška pri brisanju plana.';
        },
      });
    });
  }

  // deletePlan(id: string, name: string): void {

  //   const confirmed = confirm(
  //     `Are you sure you want to delete plan "${name}"?`
  //   );

  //   if (!confirmed) {
  //     return;
  //   }

  //   this.planService.delete(id).subscribe({
  //     next: () => {
  //       this.loadPlans();
  //     },

  //     error: (error) => {
  //       console.error('Error deleting plan:', error);

  //       this.errorMessage =
  //         'Greška pri brisanju plana.';
  //     }
  //   });
  // }
}
