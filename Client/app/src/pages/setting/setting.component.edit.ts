import { Component, ElementRef, EventEmitter, ViewChild, Input, inject, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Observable, of, finalize, BehaviorSubject } from 'rxjs';
import { Setting, emptySetting } from '../page-entities/setting.entity';
import { DdlItem } from '../page-entities/ddl-item.entity';
import { SettingService } from './setting.service';
import { jQ, hideShowModal, validateForm } from './../../shared/jquery-utils';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'edit-modal',
  template: `
<div #modal class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="display: none;">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header box-header well">
        <h2 class="modal-title" id="model-title-h2">
          <i class="fa fa-edit"></i> Edit Setting
        </h2>
        <div class="box-icon">
          <a href="#" data-dismiss="modal" data-neo-modal="true" class="btn btn-close btn-round btn-default">
            <i class="glyphicon glyphicon-remove"></i>
          </a>
        </div>
      </div>
      <form #neoEditForm="ngForm" (submit)="onSubmit(neoEditForm)" class="box neo-edit-form" novalidate="novalidate">
        <div class="modal-body">
          <div class="row edit-form-field-container">
            <input [ngModel]="obj.SettingId" type="hidden" name="SettingId" />
            <input [ngModel]="obj.CreatedOn" type="hidden" name="CreatedOn" />
            <input [ngModel]="obj.CreatedBy" type="hidden" name="CreatedBy" />
            <input [ngModel]="obj.ModifiedOn" type="hidden" name="ModifiedOn" />
            <input [ngModel]="obj.ModifiedBy" type="hidden" name="ModifiedBy" />
            <div class="col-lg-12">
              <div class="form-group">
                <label>Company <span class="field-validation-valid" data-valmsg-for="CompanyId" data-valmsg-replace="true"></span>
                </label>
                <select [(ngModel)]="obj.CompanyId"  name="CompanyId" class="form-control" data-val="true" data-val-range="The Company field is required." data-val-required="The Company field is required." data-val-range-min="1" data-val-range-max="999999">
                  <option value="0">--SELECT--</option>
                  <option value="{{item.Value}}" *ngFor="let item of companyList">{{item.Text}}</option>
                </select>
              </div>
            </div>
            <div class="col-lg-12">
              <div class="form-group">
                <label>Name <span class="field-validation-valid" data-valmsg-for="Name" data-valmsg-replace="true"></span>
                </label>
                <input [(ngModel)]="obj.Name" type="text" name="Name" placeholder="Name" class="form-control" data-val="true" data-val-required="The Name field is required." autocomplete="off" />
              </div>
            </div>
            <div class="col-lg-12">
              <div class="form-group">
                <label>Value <span class="field-validation-valid" data-valmsg-for="Value" data-valmsg-replace="true"></span>
                </label>
                <ng-container *ngIf="valueDdl.length > 0">
                  <select [(ngModel)]="obj.Value" name="Value" class="form-control" data-val="true" data-val-required="The Value field is required." autocomplete="off">
                      <option value="">--SELECT--</option>
                      <option value="{{item.Value}}" *ngFor="let item of valueDdl">{{item.Text}}</option>
                  </select>
                </ng-container>
                <ng-container *ngIf="valueDdl.length == 0">
                  <input [(ngModel)]="obj.Value" type="text" name="Value" placeholder="Value" class="form-control" data-val="true" data-val-required="The Value field is required." autocomplete="off" />
                </ng-container>
              </div>
            </div>
            <div class="col-lg-12">
              <div class="form-group">
                <label> Is Archived <span class="field-validation-valid" data-valmsg-for="IsArchived" data-valmsg-replace="true"></span> &nbsp;&nbsp;&nbsp; </label>
                <input [(ngModel)]="obj.IsArchived" type="checkbox" name="IsArchived" placeholder = "Is Archived" />
              </div>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default btn-flat fa fa-times" data-dismiss="modal">&nbsp;&nbsp;&nbsp;Close</button>
          <button type="submit" class="btn btn-info btn-flat fa fa-save">&nbsp;&nbsp;&nbsp;Save changes</button>
        </div>
        <div *ngIf="api.loading$ | async" class="overlay"></div>
        <div *ngIf="api.loading$ | async" class="loading-img"></div>
      </form>
    </div>
  </div>
</div>
  `})
export class EditComponent {
  @ViewChild('neoEditForm', { read: ElementRef }) formElement!: ElementRef;
  @ViewChild('modal', { static: false }) modal!: ElementRef;
  @Output() shouldRefresh = new EventEmitter<boolean>();
  @Input() obj: Setting = emptySetting();


  api = inject(SettingService);
  companyList: DdlItem[] = [];
  valueDdl: DdlItem[] = [];

  private getModal(): HTMLElement {
    return this.modal.nativeElement;
  }

  loadForm(obj: Setting): void {
    hideShowModal(this.getModal(), 'show');
    this.obj = obj;
    this.valueDdl = [];
    this.api.loadDropDownList('App.Service.CompanyService', obj.CompanyId).subscribe(resp => {
      this.companyList = resp;
    });
    if (obj.Name == "AppTheme") {
      this.api.loadDropDownList('Theme', obj.Value).subscribe(resp => {
        this.valueDdl = resp;
      });
    }
    else if (obj.Name == "AppNotyOptLayout") {
      this.api.loadDropDownList('NotificationOptionLayout', obj.Value).subscribe(resp => {
        this.valueDdl = resp;
      });
    }
    else if (obj.Name == "AppNotyOptType" || obj.Name == "AppNotyOptTypeError") {
      this.api.loadDropDownList('NotificationOptionType', obj.Value).subscribe(resp => {
        this.valueDdl = resp;
      });
    }
  }

  onSubmit(form: NgForm): void {
    var isValid = validateForm(this.formElement)
    if (isValid) {
      delete this.obj.Company;
      this.api.edit(this.obj).subscribe((resp) => {
        alert(resp.message);
        hideShowModal(this.getModal(), 'hide');
        this.shouldRefresh.emit(true);
      });
    }
  }
}