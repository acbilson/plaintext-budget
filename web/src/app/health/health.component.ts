import { Component, OnInit } from '@angular/core';
import { ApiStatus } from './interfaces/api-status';
import { BudgetService } from 'app/services/budget/budget.service';

@Component({
  selector: 'app-health',
  templateUrl: './health.component.html'
})
export class HealthComponent implements OnInit {
  apiStatus: ApiStatus[];

  constructor(private budgetService: BudgetService) {
    this.budgetService = budgetService;
    this.apiStatus = [];
  }

  ngOnInit() {
    this.apiStatus = [
      {
        name: 'BudgetService',
        status: 'Healthy'
      }
    ];
  }
}
