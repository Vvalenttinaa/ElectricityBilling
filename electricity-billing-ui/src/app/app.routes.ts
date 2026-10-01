import { Routes } from '@angular/router';

import { PlansListComponent } from './pages/plans/plans-list/plans-list.component';

import { PlanFormComponent } from './pages/plans/plan-form/plan-form.component';

import { PlanDetailsComponent } from './pages/plans/plan-details/plan-details.component';

import { TaxGroupsListComponent } from './pages/tax-groups/tax-groups-list/tax-groups-list.component';

import { TaxGroupFormComponent } from './pages/tax-groups/tax-group-form/tax-group-form.component';

import { TaxGroupDetailsComponent } from './pages/tax-groups/tax-group-details/tax-group-details.component';

import { RecommendationComponent } from './pages/recommendations/recommendation/recommendation.component';

import { AdminLoginComponent } from './pages/admin-login/admin-login.component';

import { AdminComponent } from './pages/admin/admin.component';

import { adminGuard } from './guards/admin.guard';
import { AnalyticsComponent } from './pages/analytics/analytics.component';

export const routes: Routes = [
  // PUBLIC HOME
  {
    path: '',
    redirectTo: 'recommendation',
    pathMatch: 'full',
  },

  // PUBLIC
  {
    path: 'recommendation',
    component: RecommendationComponent,
  },

  // PUBLIC
  {
    path: 'admin-login',
    component: AdminLoginComponent,
  },

  // ADMIN DASHBOARD
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [adminGuard],
  },

  // ADMIN - PLANS
  {
    path: 'plans',
    component: PlansListComponent,
    canActivate: [adminGuard],
  },
  {
    path: 'plans/new',
    component: PlanFormComponent,
    canActivate: [adminGuard],
  },
  {
    path: 'plans/:id/edit',
    component: PlanFormComponent,
    canActivate: [adminGuard],
  },
  {
    path: 'plans/:id',
    component: PlanDetailsComponent,
    canActivate: [adminGuard],
  },

  // ADMIN - TAX GROUPS
  {
    path: 'tax-groups',
    component: TaxGroupsListComponent,
    canActivate: [adminGuard],
  },
  {
    path: 'tax-groups/new',
    component: TaxGroupFormComponent,
    canActivate: [adminGuard],
  },
  {
    path: 'tax-groups/:id/edit',
    component: TaxGroupFormComponent,
    canActivate: [adminGuard],
  },
  {
    path: 'tax-groups/:id',
    component: TaxGroupDetailsComponent,
    canActivate: [adminGuard],
  },

  {
    path: 'admin/analytics',
    component: AnalyticsComponent,
    canActivate: [adminGuard],
  },

  // FALLBACK
  {
    path: '**',
    redirectTo: 'recommendation',
  },
];
