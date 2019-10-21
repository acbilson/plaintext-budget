import { Component, OnInit } from '@angular/core';
import { BudgetService } from 'app/services/budget/budget.service';
import { LoggingService } from 'app/services/logging/logging.service';

@Component({
  selector: 'app-budget-page',
  templateUrl: './budget-page.component.html',
  styleUrls: ['./budget-page.component.css']
})
export class BudgetPageComponent implements OnInit {
  private context: string;

  constructor(
    private budgetService: BudgetService,
    private logger: LoggingService
  ) {
    this.budgetService = budgetService;
    this.logger = logger;
    this.context = 'budget-page';
  }

  ngOnInit() {}
}
