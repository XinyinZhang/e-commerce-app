import { CdkStep, CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent implements OnInit {

  @Input() appStepper: CdkStepper; // passed by checkoutComponent to enable the move
  // to next step functionality

  basket$: Observable<IBasket>;
  constructor(private basketService: BasketService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }
  // tslint:disable-next-line: typedef
  createPaymentIntent() {
    return this.basketService.createPaymentIntent().subscribe((response: any) => {
      // this.toastr.success('Payment intent created');
      this.appStepper.next(); // after creating the payment, move to the Payment step
    }, error => {
      console.log(error);
      // this.toastr.error(error.message);
    });
  }

}
