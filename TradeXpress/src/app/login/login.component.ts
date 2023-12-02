import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  fieldTextType: boolean;
constructor(public authService: AuthService){}
  
email: string = 'gramwfortnitebr@gmail.com';
password: string = '12345678';
isWrongEmail: boolean = false;
isWrongPassword: boolean = false;
public showPassword: boolean;

toggleFieldTextType() {
  this.fieldTextType = !this.fieldTextType;
}

onSubmit()
{
 
  console.log(this.email,this.password);
  var validRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

  if (!this.email.match(validRegex)) {
    console.log("git");
    this.isWrongEmail = true;
  }
  else
  {
    this.isWrongEmail = false;
  }

  if(this.password.length < 8)
  {
   
    this.isWrongPassword = true;
  }
  else
  {
    this.isWrongPassword = false;
  }

  if(!this.isWrongEmail && !this.isWrongPassword)
  {
    this.authService.SignIn(this.email,this.password);
  }
}

}
