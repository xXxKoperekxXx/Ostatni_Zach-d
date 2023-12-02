import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Active, ActiveUsersServiceService } from '../services/active-users-service.service';
import { User } from '../services/user';
import { BetsService, bet } from '../services/bets.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {

  randomProducts : any;
  activeUsers: any[] = []; 
  currentUser:any;
  balance: number;
  selectedUser: Active | undefined;
  placedAbet: boolean = false;
  started: boolean = false;
  top3bets:bet[];
  constructor(public authService: AuthService,public activeUsersService: ActiveUsersServiceService, public router: Router, public betsService: BetsService) { 
    

  }

  ngOnInit()
  {
    
    this.authService.getUserInformation().subscribe(user => {
      this.currentUser = user.uid;
      //console.log(this.currentUser);
      this.authService.getUserBalance(this.currentUser).subscribe(balance => {
        this.balance = balance;
       // console.log('User Balance:', this.balance);
      });


      this.activeUsers = [];
      //console.log('puste');
this.a();
     this.get3bets();
    //console.log(this.activeUsers);

    })  
  }

  a()
  {
    //console.log("a");
    this.activeUsers = [];
    this.activeUsersService.getActiveUsers().subscribe((users) => {
      //console.log('actiwny', this.activeUsers);
      //console.log(users);
      this.activeUsers = users;
      console.log('Active Users:', this.activeUsers);
    });
  } 

  update(event: any) {
    const selectedNickname = event.target.value;
    this.selectedUser = this.activeUsers.find(user => user.nickname === selectedNickname);
  }

  saveChoice()
  {
    this.placedAbet = true;
    let user = this.authService.userData;

    console.log("WYbrano usera: ",this.selectedUser,"ty jestes: ", user.displayName);


    this.betsService.placeAbet(user.displayName,this.selectedUser);
  }

  ngOnDestroy()
  {
    this.activeUsersService.deleteActiveUser(this.currentUser.displayName);
  }



  get3bets()
  {
    
    this.top3bets = [];
    this.betsService.get3LatestsBets().subscribe((bets) => {
    
      if (this.hasDifferentUsers(bets) && bets.length === 3) {
        this.top3bets = bets;
        this.started = true;
        console.log('hier');
      } else {
        // Handle the case where not all bets have different users
        console.error('Not all bets have different users.');
      }
      console.log('top3bets', this.top3bets);

    });
  }

  hasDifferentUsers(bets: bet[]): boolean {
    const uniqueUsers = new Set<string>();

    for (const bet of bets) {
      if (uniqueUsers.has(bet.user)) {
        // Duplicate user found
        return false;
      } else {
        uniqueUsers.add(bet.user);
      }
    }

    // No duplicate users found
    return true;
  }
}
