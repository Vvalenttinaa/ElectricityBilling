import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { Plan } from '../../../models/plan.model';
import { PlanService } from '../../../services/plan.service';

@Component({
  selector: 'app-plan-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './plan-details.component.html',
  styleUrl: './plan-details.component.scss',
})
export class PlanDetailsComponent implements OnInit {
  plan?: Plan;

  loading = true;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private planService: PlanService,
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.errorMessage = 'Plan ID nije pronađen.';
      this.loading = false;
      return;
    }

    this.loadPlan(id);
  }

  loadPlan(id: string): void {
    this.loading = true;
    this.errorMessage = '';

    this.planService.getById(id).subscribe({
      next: (plan) => {
        this.plan = plan;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading plan:', error);
        this.errorMessage = 'Greška pri učitavanju plana.';
        this.loading = false;
      },
    });
  }

  deletePlan(): void {
    if (!this.plan) {
      return;
    }

    const confirmed = confirm(
      `Are you sure you want to delete plan "${this.plan.name}"?`,
    );

    if (!confirmed) {
      return;
    }

    this.planService.delete(this.plan.id).subscribe({
      next: () => {
        this.router.navigate(['/plans']);
      },
      error: (error) => {
        console.error('Error deleting plan:', error);
        this.errorMessage = 'Greška pri brisanju plana.';
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/plans']);
  }
}
