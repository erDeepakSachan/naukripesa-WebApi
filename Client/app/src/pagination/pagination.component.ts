import { Component, ElementRef, AfterViewInit, ViewChild, Output, EventEmitter, Input, inject, OnInit, OnDestroy } from '@angular/core';
import { initPagination } from '../shared/jquery-utils';

@Component({
  selector: 'app-pagination',
  template: `       
   <div #paginationContainer [attr.data-pagination-id]="groupName" [className]="className"></div>`,
})
export class PaginationComponent implements AfterViewInit {
  @ViewChild('paginationContainer', { static: true }) paginationRef!: ElementRef;
  @Output() pageChange = new EventEmitter<number>();
  @Input() className: string = '';
  @Input() groupName: string = '';

  state: any = {};

  ngAfterViewInit(): void {
    this.reInitPagination(1);
  }

  reInitPagination(pageNo: number): void {
    initPagination(this.groupName, 1, pageNo, (page: number) => {
      this.pageChange.emit(page);
    });
  }
}