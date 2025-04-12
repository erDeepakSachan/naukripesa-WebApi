import { Component, ElementRef, EventEmitter, ViewChild, Input, inject, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { jQ, hideShowModal, validateForm, removeValidationErrors } from './../../shared/jquery-utils';
import { Setting, emptySetting } from '../page-entities/setting.entity';
import { SettingService } from './setting.service';
import { DdlItem } from '../page-entities/ddl-item.entity';

@Component({
    imports: [CommonModule, FormsModule],
    selector: 'add-modal',
    template: `
        <div #modal class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="display: none;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header box-header well">
                    <h2 class="modal-title" id="model-title-h2"><i class="fa fa-plus"></i> Add Setting</h2>
                    <div class="box-icon">
                        <a href="#" data-dismiss="modal" data-neo-modal="true" class="btn btn-close btn-round btn-default">
                            <i class="glyphicon glyphicon-remove"></i>
                        </a>
                    </div>
                </div>
                <form #neoAddForm="ngForm" (submit)="onSubmit(neoAddForm)" class="box neo-add-form" novalidate>
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
                            <select [(ngModel)]="obj.CompanyId" name="CompanyId" class="form-control" data-val="true" data-val-range="The Company field is required." data-val-required="The Company field is required." data-val-range-min="1" data-val-range-max="999999">
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
                                <input [(ngModel)]="obj.Value" type="text" name="Value" placeholder="Value" class="form-control" data-val="true" data-val-required="The Value field is required." autocomplete="off"  />
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
export class AddComponent {
    @ViewChild('neoAddForm', { read: ElementRef }) formElement!: ElementRef;
    @ViewChild('modal', { static: false }) modal!: ElementRef;
    @Output() shouldRefresh = new EventEmitter<boolean>();
    @Input() obj: Setting = emptySetting();

    api = inject(SettingService);
    companyList: DdlItem[] = [];

    private getModal(): HTMLElement {
        return this.modal.nativeElement;
    }

    loadForm(): void {
        removeValidationErrors(this.formElement);
        this.obj = emptySetting();
        hideShowModal(this.getModal(), 'show');
        this.api.loadDropDownList('App.Service.CompanyService', '0').subscribe(resp => {
            this.companyList = resp;
        });
    }

    onSubmit(form: NgForm): void {
        var isValid = validateForm(this.formElement)
        if (isValid) {
            delete this.obj.Company;
            this.api.add(this.obj).subscribe((resp) => {
                alert(resp.message);
                hideShowModal(this.getModal(), 'hide');
                this.shouldRefresh.emit(true);
            });
        }
    }
}