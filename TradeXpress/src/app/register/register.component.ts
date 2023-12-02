import { Component } from '@angular/core';
import { AuthService } from "../services/auth.service";
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent{
  fieldTextType: boolean;
  fieldTextType2: boolean;
  constructor(public authService: AuthService)
  {
    
  }
  
  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }
  toggleFieldTextType2() {
    this.fieldTextType2 = !this.fieldTextType2;
  }
  email: string;
  nickname: string;
password: string;
password2: string;
isWrongEmail: boolean = false;
isWrongPassword: boolean = false;


onSubmit()
{
 
  console.log(this.email,this.password, this.password2);
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

  if(this.password2 == this.password)
  {
    this.isWrongPassword = false;
  }
  else
  {
    this.isWrongPassword = true;
  }

  if(!this.isWrongEmail && !this.isWrongPassword)
  {
    this.authService.SignUp(this.email,this.password,this.nickname);
  }
}
}
