import { Injectable } from '@angular/core';
import { AngularFirestore, AngularFirestoreCollection, AngularFirestoreDocument } from '@angular/fire/compat/firestore';
import { Observable, map } from 'rxjs';


export interface Active {
  nickname:string;
}

@Injectable({
  providedIn: 'root'
})
export class ActiveUsersServiceService {

  activeusers: [any];
  public lastnick:string = "";
  private collectionName: string = 'active_users';
  private collectionRef: AngularFirestoreCollection<Active>;

  constructor( public afs: AngularFirestore) { 
    this.collectionRef = this.afs.collection<Active>(this.collectionName);
  }


  addActiveUser(nick)
  {
   if(this.lastnick !== nick)
   {
    this.lastnick = nick;
   // console.log('Dodano')
   // console.log('nick: ' ,nick);
    const player: Active = {nickname: nick}
    this.collectionRef.add(player);  
   }
   else
   {
   // console.log("istnieje juz");
   }
    
  }

  deleteActiveUser(nick)
  {
    this.lastnick='';
    console.log('usunieto');
    console.log('nick: ' ,nick);
    console.log(nick);
    const query = this.collectionRef.ref.where('nickname', '==', nick);

    // Get documents based on the query
    query.get().then((querySnapshot) => {
      querySnapshot.forEach((doc) => {
        // Delete each document
        this.collectionRef.doc(doc.id).delete();
      });
    });
  }

  getActiveUsers(): Observable<any[]>
  {
    return this.collectionRef.valueChanges().pipe(
      map(activeUsers => {
        return activeUsers;
      })
    );
  }
}
