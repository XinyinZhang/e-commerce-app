import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { threadId } from 'worker_threads';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  constructor(private fb: FormBuilder, private accountService: AccountService,
              private basketService: BasketService) { }

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddressFormValues(); // load user's default address automatically
    this.getDeliveryMethodValue();
  }
  createCheckoutForm(): void{
    this.checkoutForm = this.fb.group({
      addressForm: this.fb.group({
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        street: [null, Validators.required],
        city: [null, Validators.required],
        state: [null, Validators.required],
        zipcode: [null, Validators.required],
      }),
      deliveryForm: this.fb.group({
        deliveryMethod: [null, Validators.required]
      }),
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required]
      })
    });
  }

  // tslint:disable-next-line: typedef
  getAddressFormValues() {
    this.accountService.getUserAddress().subscribe(address => {
      // check if current user has an address --> if so, populate the form
      // with the address value
      if (address) {
        this.checkoutForm.get('addressForm').patchValue(address);
      }
    }, error => {
      console.log(error);
    });
  }

  // tslint:disable-next-line: typedef
  getDeliveryMethodValue() {
    const basket = this.basketService.getCurrentBasketValue();
    if (basket.deliveryMethodId !== null) {
      this.checkoutForm.get('deliveryForm')
      .get('deliveryMethod').patchValue(basket.deliveryMethodId.toString());
    }
  }

}
