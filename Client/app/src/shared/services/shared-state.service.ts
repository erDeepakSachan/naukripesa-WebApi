import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedStateService {
  private stateGroups: { [key: string]: BehaviorSubject<any> } = {};
  private groupUsageCount: { [key: string]: number } = {};

  getState(group: string) {
    if (!this.stateGroups[group]) {
      this.stateGroups[group] = new BehaviorSubject<any>({});
      this.groupUsageCount[group] = 0;
    }
    this.groupUsageCount[group]++;
    return this.stateGroups[group].asObservable();
  }

  updateState(group: string, newState: any) {
    if (this.stateGroups[group]) {
      this.stateGroups[group].next(newState);
    }
  }

  removeGroup(group: string) {
    if (this.groupUsageCount[group]) {
      this.groupUsageCount[group]--;

      if (this.groupUsageCount[group] === 0) {
        delete this.stateGroups[group];
        delete this.groupUsageCount[group];
      }
    }
  }
}