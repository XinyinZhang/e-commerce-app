import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-stepper',
  templateUrl: './stepper.component.html',
  styleUrls: ['./stepper.component.scss'],
  providers: [{provide: CdkStepper, useExisting: StepperComponent}]
})
export class StepperComponent extends CdkStepper implements OnInit {
  // an input property to let the client/child component tell us
  // whether or not the linear mode is selected
  // EnableLinearMode: the client can click on the next step even though the previous step is blank
  // DisableLinearMode: the client can't go to next step until the previous step is completed
  @Input() linearModeSelected: boolean;

  ngOnInit(): void {
    // Whether the validity of previous steps should be checked or not: depends on whether
    // client turn linearMode on/off
    this.linear = this.linearModeSelected;
  }
  // a onClick event to keep track of which step we are currently on
  onClick(index: number): void {
    this.selectedIndex = index; // the index of the selected step
  }
}
