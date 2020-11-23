import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { IOrder } from 'src/app/shared/models/order';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  constructor(private basketService: BasketService,
              private checkoutService: CheckoutService,
              private toastr: ToastrService,
              private router: Router) { }

  ngOnInit(): void {
  }
  submitOrder(): void {
    const basket = this.basketService.getCurrentBasketValue();
    const orderToCreate = this.getOrderToCreate(basket);
    this.checkoutService.creatOrder(orderToCreate).subscribe((order: IOrder) => {
      this.toastr.success('Order created successfully');
      // after successfully create the order, remove the basket locally
      this.basketService.deleteLocalBasket(basket.id);
      // after successfully create the order, direct user to success page
      const navigationExtras: NavigationExtras = {state: order};
      this.router.navigate(['checkout/success'], navigationExtras);
    }, error => {
      this.toastr.error(error.message);
      console.log(error);
    });
  }
  // use the information collected from the previous steps
  // and create an order
  // tslint:disable-next-line: typedef
  private getOrderToCreate(basket: IBasket) {
    return {
      basketId: basket.id,
      // +: we want a number
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value
    };
  }

}
