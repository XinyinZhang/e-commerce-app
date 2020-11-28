import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IAddress } from 'src/app/shared/models/address';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  constructor(private accountService: AccountService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
  }
  saveUserAddress(): void{
    // after user fill in the address form and click saveToDefaultAddress button,
    // the new address will be updated to server database
    // the next time user load this page, the new address will be loaded automatically
    this.accountService.updateUserAddress(this.checkoutForm.get('addressForm').value)
      .subscribe((address: IAddress) => {
        this.toastr.success('Address saved');
        this.checkoutForm.get('addressForm').reset(address);
      }, error => {
        this.toastr.error(error.message);
        console.log(error);
      });
  }

}
