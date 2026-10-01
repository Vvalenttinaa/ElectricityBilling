import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';

import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';

import { AnalyticsService } from '../../services/analytics.service';
import { AnalyticsSummary } from '../../models/analytics.model';

import {
  TranslatePipe,
  TranslateService
} from '@ngx-translate/core';

@Component({
  selector: 'app-analytics',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
    BaseChartDirective,
    TranslatePipe
  ],
  templateUrl: './analytics.component.html',
  styleUrl: './analytics.component.scss',
})
export class AnalyticsComponent implements OnInit {

  analytics?: AnalyticsSummary;

  loading = true;
  errorMessage = '';

  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [
      {
        label: '',
        data: [],
      },
    ],
  };

  barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,

    plugins: {
      legend: {
        display: false,
      },
    },

    scales: {
      y: {
        beginAtZero: true,
        ticks: {
          stepSize: 1,
        },
        title: {
          display: true,
          text: '',
        },
      },

      x: {
        title: {
          display: true,
          text: '',
        },
      },
    },
  };

  constructor(
    private analyticsService: AnalyticsService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.loadAnalytics();

    this.translate.onLangChange.subscribe(() => {
      this.updateChartTranslations();

      if (this.errorMessage) {
        this.setErrorMessage();
      }
    });
  }

  loadAnalytics(): void {
    this.loading = true;
    this.errorMessage = '';

    this.analyticsService.getSummary().subscribe({
      next: (data) => {
        this.analytics = data;

        this.barChartData = {
          labels: data.planSelectionTrends.map(
            (x) => x.planName
          ),

          datasets: [
            {
              label: '',
              data: data.planSelectionTrends.map(
                (x) => x.selectionCount
              ),
            },
          ],
        };

        this.updateChartTranslations();

        this.loading = false;
      },

      error: (error) => {
        console.error(
          'Error loading analytics:',
          error
        );

        this.setErrorMessage();

        this.loading = false;
      },
    });
  }

  private updateChartTranslations(): void {
    this.translate
      .get([
        'DASHBOARD.NUMBER_OF_RECOMMENDATIONS',
        'DASHBOARD.RECOMMENDATIONS',
        'DASHBOARD.PLANS',
      ])
      .subscribe((translations) => {

        this.barChartData = {
          ...this.barChartData,

          datasets: this.barChartData.datasets.map(
            (dataset) => ({
              ...dataset,

              label:
                translations[
                  'DASHBOARD.NUMBER_OF_RECOMMENDATIONS'
                ],
            })
          ),
        };

        this.barChartOptions = {
          responsive: true,
          maintainAspectRatio: false,

          plugins: {
            legend: {
              display: false,
            },
          },

          scales: {
            y: {
              beginAtZero: true,

              ticks: {
                stepSize: 1,
              },

              title: {
                display: true,
                text:
                  translations[
                    'DASHBOARD.RECOMMENDATIONS'
                  ],
              },
            },

            x: {
              title: {
                display: true,
                text:
                  translations[
                    'DASHBOARD.PLANS'
                  ],
              },
            },
          },
        };
      });
  }

  private setErrorMessage(): void {
    this.translate
      .get('DASHBOARD.LOAD_ERROR')
      .subscribe((message) => {
        this.errorMessage = message;
      });
  }
}