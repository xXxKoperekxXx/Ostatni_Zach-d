import { Injectable } from '@angular/core';
import { AngularFirestore, AngularFirestoreCollection, AngularFirestoreDocument } from '@angular/fire/compat/firestore';
import { Observable, map } from 'rxjs';

export interface bet {
  WhoWillWin:string;
  user:string;
}

@Injectable({
  providedIn: 'root'
})
export class BetsService {

  private collectionName: string = 'bets';
  private collectionRef: AngularFirestoreCollection<bet>;
  
  constructor( public afs: AngularFirestore) {
    this.collectionRef = this.afs.collection<bet>(this.collectionName);
   }



   placeAbet(user, favourite)
   {
    const newBet: bet= {WhoWillWin:favourite.nickname, user:user};
    this.collectionRef.add(newBet);

   }

   get3LatestsBets(): Observable<bet[]>
   {
    return this.collectionRef.valueChanges().pipe(
        map(bets => {
          const top3bets:bet[] = bets.slice(0, 3);
          return top3bets;
        })
      );
   }
}
