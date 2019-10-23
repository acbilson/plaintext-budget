import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, Input } from '@angular/core';
import { BudgetService } from 'app/services/budget/budget.service';
import { LoggingService } from 'app/services/logging/logging.service';
import { Row } from 'app/interfaces/row';
import { ReportSchema } from 'app/interfaces/report-schema';
import { SchemaService } from 'app/services/schema/schema.service';
import { BudgetColumn } from 'app/interfaces/budget-column';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BudgetEntry } from 'app/interfaces/budget-entry';
import { BudgetEntryType } from 'app/interfaces/budget-entry-type';
import { BudgetForm } from 'app/interfaces/budget-form';

@Component({
  selector: 'app-budget-form',
  templateUrl: './budget-form.component.html',
  styleUrls: ['./budget-form.component.css']
})
export class BudgetFormComponent implements OnInit {
  private logger: LoggingService;
  private context: string;
  public budgetEntries: BudgetEntry[];

  // necessary to use enum in template
  public BudgetEntryType = BudgetEntryType;

  @Input() budgetName: string;

  constructor(
    private budgetService: BudgetService,
    private loggingService: LoggingService,
    private route: ActivatedRoute
  ) {
    this.budgetService = budgetService;
    this.logger = loggingService;
    this.route = route;
    this.context = 'budget-form';
    this.budgetEntries = [];
  }

  ngOnInit() {
    this.budgetName = this.route.snapshot.paramMap.get('name');
    this.readBudget(this.budgetName);
  }

  onEdit(e) {
    const indexToUpdate = this.budgetEntries.findIndex(
      entry => entry.id === parseInt(e.target.id, 10)
    );
    console.log(indexToUpdate);
    this.budgetEntries[indexToUpdate].amount.value = e.target.value;
  }

  onSave(e) {
    console.log(e);
    const response = this.budgetService.update();
  }

  async readBudget(fileName: string) {
    this.logger.logInfo(this.context, `reading from budget ${fileName}`);

    try {
      this.budgetEntries = await this.budgetService.read(fileName);
    } catch (error) {
      this.logger.logError(
        this.context,
        `failed to retrieve budget with message: ${error.message}`
      );
    }
  }
}
