import { Injectable } from '@angular/core';
import { User } from '../services/user';
import * as auth from 'firebase/auth';
import { AngularFireAuth } from '@angular/fire/compat/auth';
;
import { Router } from '@angular/router';

import { AngularFirestore, AngularFirestoreDocument } from '@angular/fire/compat/firestore';

import { ActiveUsersServiceService } from './active-users-service.service';
import { getAuth, updateProfile } from "firebase/auth";
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  userData: any;

  
  constructor(
    public afs: AngularFirestore, // Inject Firestore service
    public afAuth: AngularFireAuth, // Inject Firebase auth service
    public router2: Router,
    public activeUsersService: ActiveUsersServiceService) {

    this.afAuth.authState.subscribe((user) => {
      console.log("authstatechanged");
      if (user) {
        //console.log(user);
        this.userData = user;
        localStorage.setItem('user', JSON.stringify(this.userData));

        JSON.parse(localStorage.getItem('user')!);
        //this.SignOut();
      } else {
        localStorage.setItem('user', 'null');
        JSON.parse(localStorage.getItem('user')!);
      }
    });

  }

  getUserInformation() 
{
  //console.log(this.afAuth.authState);
  return this.afAuth.authState.pipe(map(user =>{
    return {
      uid: user.uid,
      email: user.email,
      displayName: user.displayName,
      emailVerified: user.emailVerified
    } as User
  }));
}

  async SetUserData() {

    const auth = getAuth();
    var user = auth.currentUser;
    if (user) {
      this.userData = user;
      localStorage.setItem('user', JSON.stringify(this.userData));

      JSON.parse(localStorage.getItem('user')!);
      //this.SignOut();
    }
    console.log(user.displayName, user.uid, user.email, user.emailVerified);

    const userRef: AngularFirestoreDocument<any> = this.afs.doc(
      `users/${user.uid}`
    );
    const userData = {
      uid: user.uid,
      email: user.email,
      displayName: user.displayName,
      emailVerified: user.emailVerified
    };
   await userRef.set(userData, {
      merge: true,
    });
  }
  
  get isLoggedIn(): boolean {
  const user = JSON.parse(localStorage.getItem('user')!);
  return user !== null && user.emailVerified !== false ? true : false;
}

SignOut() {

  this.router2.navigate(['login']);
  this.activeUsersService.deleteActiveUser(this.userData.displayName);
this.userData='';
  return this.afAuth.signOut().then(() => {
    localStorage.removeItem('user');
    this.router2.navigate(['login']);
  });
}



async SignIn(email: string, password: string) {
  await this.afAuth
    .signInWithEmailAndPassword(email, password)
    .then(() => {
      //console.log("signin");
      this.SetUserData();
      this.afAuth.authState.subscribe((user) => {
        //console.log("wszedl");
        if (user) {
        
          this.activeUsersService.addActiveUser(user.displayName);
          //console.log("nawiagacja do dash");
          this.router2.navigate(['dashboard']);
        }
      });
    })
    .catch((error) => {
      window.alert(error.message);
    });
}
// Sign up with email/password
SignUp(email: string, password: string, nickname: string) {
  return this.afAuth
    .createUserWithEmailAndPassword(email, password)
    .then((result) => {
      this.UpdateProfile(nickname);
      this.SendVerificationMail();
     
    })
    .catch((error) => {
      window.alert(error.message);
    });
}

  async UpdateProfile(nickname: string) {

  const auth = getAuth();
  updateProfile(auth.currentUser, {
    displayName: nickname
  }).then(() => {
    this.SetUserData();
   
  }).catch((error) => {
  });

}

SendVerificationMail() {
  return this.afAuth.currentUser
    .then((u: any) => u.sendEmailVerification())
    .then(() => {
      this.router2.navigate(['verify-email-address']);
    });
}



getUserBalance(uid)
{

  //console.log(`users/${uid}`);
  const userRef: AngularFirestoreDocument<any> = this.afs.doc(
    `users/${uid}`
  );
  
  return userRef.valueChanges().pipe(
    map((user: any) => {
      if (user) {
        return user.balance;
      } else {
        return 0; // Or any default value if the user is not found
      }
    })
  );

}

addBalance()
{
  const auth = getAuth();
  var user = auth.currentUser;

  const userRef: AngularFirestoreDocument<any> = this.afs.doc(
    `users/${user.uid}`
  );

  userRef.update({balance : 100});

}



}
