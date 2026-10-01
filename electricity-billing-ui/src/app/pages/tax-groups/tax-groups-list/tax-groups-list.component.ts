import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

import { TaxGroup } from '../../../models/tax-group.model';
import { TaxGroupService } from '../../../services/tax-group.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { ConfirmDialogComponent } from '../../../shared/confirm-dialog/confirm-dialog.component';
import { TranslatePipe } from '@ngx-translate/core';
@Component({
  selector: 'app-tax-groups-list',
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
  templateUrl: './tax-groups-list.component.html',
  styleUrl: './tax-groups-list.component.scss',
})
export class TaxGroupsListComponent implements OnInit {
  taxGroups: TaxGroup[] = [];

  loading = true;
  errorMessage = '';

  displayedColumns: string[] = [
    'name',
    'description',
    'vat',
    'ecoTax',
    'status',
    'actions',
  ];

  constructor(
    private taxGroupService: TaxGroupService,
    private dialog: MatDialog,
  ) {}

  ngOnInit(): void {
    this.loadTaxGroups();
  }

  loadTaxGroups(): void {
    this.loading = true;
    this.errorMessage = '';

    this.taxGroupService.getAll().subscribe({
      next: (data) => {
        this.taxGroups = data;
        this.loading = false;
      },

      error: (error) => {
        console.error(error);

        this.errorMessage = 'Greška pri učitavanju poreskih grupa.';

        this.loading = false;
      },
    });
  }

  deleteTaxGroup(id: string, name: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '420px',

      data: {
        title: 'Delete Tax Group',
        message: `Are you sure you want to delete tax group "${name}"?`,
        confirmText: 'Delete',
        cancelText: 'Cancel',
      },
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.taxGroupService.delete(id).subscribe({
        next: () => {
          this.loadTaxGroups();
        },

        error: (error) => {
          console.error(error);

          this.errorMessage = 'Greška pri brisanju poreske grupe.';
        },
      });
    });
  }

  // deleteTaxGroup(
  //   id: string,
  //   name: string
  // ): void {

  //   const confirmed = confirm(
  //     `Are you sure you want to delete tax group "${name}"?`
  //   );

  //   if (!confirmed) {
  //     return;
  //   }

  //   this.taxGroupService.delete(id).subscribe({
  //     next: () => {
  //       this.loadTaxGroups();
  //     },

  //     error: (error) => {
  //       console.error(error);

  //       this.errorMessage =
  //         'Greška pri brisanju poreske grupe.';
  //     }
  //   });
  // }
}
