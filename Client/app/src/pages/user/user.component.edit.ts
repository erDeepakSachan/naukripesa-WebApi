import {
  Component,
  ElementRef,
  EventEmitter,
  ViewChild,
  Input,
  inject,
  Output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Observable, of, finalize, BehaviorSubject } from 'rxjs';
import { User, emptyUser } from '../page-entities/user.entity';
import { DdlItem } from '../page-entities/ddl-item.entity';
import { UserService } from './user.service';
import {
  jQ,
  hideShowModal,
  validateForm,
  removeValidationErrors,
} from './../../shared/jquery-utils';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'edit-modal',
  template: `
    <div
      #modal
      class="modal fade"
      data-backdrop="static"
      data-keyboard="false"
      tabindex="-1"
      role="dialog"
      aria-hidden="true"
      style="display: none;"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header box-header well">
            <h2 class="modal-title" id="model-title-h2">
              <i class="fa fa-edit"></i> Edit User
            </h2>
            <div class="box-icon">
              <a
                href="#"
                data-dismiss="modal"
                data-neo-modal="true"
                class="btn btn-close btn-round btn-default"
              >
                <i class="glyphicon glyphicon-remove"></i>
              </a>
            </div>
          </div>
          <form
            #neoEditForm="ngForm"
            (submit)="onSubmit(neoEditForm)"
            class="box neo-edit-form"
            novalidate="novalidate"
          >
            <div class="modal-body">
              <div class="row edit-form-field-container">
                <input [(ngModel)]="obj.UserId" type="hidden" name="UserId" />
                <ul class="nav nav-tabs" role="tablist">
                  <li class="active">
                    <a href="#basic-info" role="tab" data-toggle="tab"
                      >Basic Info</a
                    >
                  </li>
                  <li>
                    <a href="#group-info" role="tab" data-toggle="tab"
                      >Group Info</a
                    >
                  </li>

                  <li>
                    <a href="#other-info" role="tab" data-toggle="tab"
                      >Other Info</a
                    >
                  </li>
                </ul>

                <div class="tab-content">
                  <div class="tab-pane" id="group-info">
                  <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >Company
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="CompanyId"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <select
                      #appIcon
                      [(ngModel)]="obj.CompanyId"
                      name="CompanyId"
                      class="form-control"
                      data-val="true"
                      data-val-range="*required."
                      data-val-required="*required."
                      data-val-range-min="1"
                      data-val-range-max="999999"
                    >
                      <option
                        value="0"
                      >
                        --SELECT--
                      </option>
                      <option
                        [attr.data-html]="item.Data"
                        value="{{ item.Value }}"
                        *ngFor="let item of companyList"
                      >
                        {{ item.Text }}
                      </option>
                    </select>
                  </div>
                  </div>
                  <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >User Type
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="UserTypeId"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <select
                      #appIcon
                      [(ngModel)]="obj.UserTypeId"
                      name="UserTypeId"
                      class="form-control"
                      data-val="true"
                      data-val-range="*required."
                      data-val-required="*required."
                      data-val-range-min="1"
                      data-val-range-max="999999"
                    >
                      <option
                        value="0"
                      >
                        --SELECT--
                      </option>
                      <option
                        [attr.data-html]="item.Data"
                        value="{{ item.Value }}"
                        *ngFor="let item of userTypeList"
                      >
                        {{ item.Text }}
                      </option>
                    </select>
                  </div>
                  </div>
                  <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >User Group
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="UserGroupId"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <select
                      #appIcon
                      [(ngModel)]="obj.UserGroupId"
                      name="UserGroupId"
                      class="form-control"
                      data-val="true"
                      data-val-range="*required."
                      data-val-required="*required."
                      data-val-range-min="1"
                      data-val-range-max="999999"
                    >
                      <option
                        value="0"
                      >
                        --SELECT--
                      </option>
                      <option
                        [attr.data-html]="item.Data"
                        value="{{ item.Value }}"
                        *ngFor="let item of userGroupList"
                      >
                        {{ item.Text }}
                      </option>
                    </select>
                  </div>
                  </div>
                  </div>
                  <div class="tab-pane active" id="basic-info">
                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Code
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="Code"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input
                          [(ngModel)]="obj.Code"
                          type="text"
                          name="Code"
                          placeholder="Code"
                          class="form-control"
                          data-val="true"
                          data-val-required="*required."
                          autocomplete="off"
                        />
                      </div>
                    </div>

                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Name
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="Name"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input
                          [(ngModel)]="obj.Name"
                          type="text"
                          name="Name"
                          placeholder="Name"
                          class="form-control"
                          data-val="true"
                          data-val-required="*required."
                          autocomplete="off"
                        />
                      </div>
                    </div>

                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Email
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="Email"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input
                          [(ngModel)]="obj.Email"
                          type="text"
                          name="Email"
                          placeholder="Email"
                          class="form-control"
                          data-val="true"
                          data-val-required="*required."
                          autocomplete="off"
                        />
                      </div>
                    </div>

                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Mobile No
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="MobileNo"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input
                          [(ngModel)]="obj.MobileNo"
                          type="text"
                          name="MobileNo"
                          placeholder="MobileNo"
                          class="form-control"
                          data-val="true"
                          data-val-required="*required."
                          autocomplete="off"
                        />
                      </div>
                    </div>
                  </div>
                  <div class="tab-pane" id="other-info">
                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Recent Login
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="RecentLogin"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input readonly
                          [(ngModel)]="obj.RecentLogin"
                          type="text"
                          name="RecentLogin"
                          placeholder="RecentLogin"
                          class="form-control"
                          autocomplete="off"
                        />
                      </div>
                    </div>
                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Created On
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="CreatedOn"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input
                          readonly
                          [(ngModel)]="obj.CreatedOn"
                          type="text"
                          name="CreatedOn"
                          placeholder="CreatedOn"
                          class="form-control"
                          data-val="true"
                          data-val-required="The CreatedOn field is required."
                          autocomplete="off"
                        />
                      </div>
                    </div>

                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Created By
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="CreatedBy"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input
                          readonly
                          [(ngModel)]="obj.CreatedBy"
                          type="text"
                          name="CreatedBy"
                          placeholder="CreatedBy"
                          class="form-control"
                          data-val="true"
                          data-val-required="The CreatedBy field is required."
                          autocomplete="off"
                        />
                      </div>
                    </div>

                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Modified On
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="ModifiedOn"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input
                          readonly
                          [(ngModel)]="obj.ModifiedOn"
                          type="text"
                          name="ModifiedOn"
                          placeholder="ModifiedOn"
                          class="form-control"
                          data-val="true"
                          data-val-required="The ModifiedOn field is required."
                          autocomplete="off"
                        />
                      </div>
                    </div>

                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Modified By
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="ModifiedBy"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input
                          readonly
                          [(ngModel)]="obj.ModifiedBy"
                          type="text"
                          name="ModifiedBy"
                          placeholder="ModifiedBy"
                          class="form-control"
                          data-val="true"
                          data-val-required="The ModifiedBy field is required."
                          autocomplete="off"
                        />
                      </div>
                    </div>

                    <div class="col-lg-12">
                      <div class="form-group">
                        <label
                          >Is Archived? &nbsp;&nbsp;&nbsp;
                          <span
                            class="field-validation-valid"
                            data-valmsg-for="IsArchived"
                            data-valmsg-replace="true"
                          ></span>
                        </label>
                        <input
                          [(ngModel)]="obj.IsArchived"
                          type="checkbox"
                          name="IsArchived"
                          placeholder="IsArchived"
                          autocomplete="off"
                        />
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-default btn-flat fa fa-times"
                data-dismiss="modal"
              >
                &nbsp;&nbsp;&nbsp;Close
              </button>
              <button type="submit" class="btn btn-info btn-flat fa fa-save">
                &nbsp;&nbsp;&nbsp;Save changes
              </button>
            </div>
            <div *ngIf="api.loading$ | async" class="overlay"></div>
            <div *ngIf="api.loading$ | async" class="loading-img"></div>
          </form>
        </div>
      </div>
    </div>
  `,
})
export class EditComponent {
  @ViewChild('neoEditForm', { read: ElementRef }) formElement!: ElementRef;
  @ViewChild('modal', { static: false }) modal!: ElementRef;
  @Output() shouldRefresh = new EventEmitter<boolean>();
  @Input() obj: User = emptyUser();

  api = inject(UserService);
  companyList: DdlItem[] = [];
  userTypeList: DdlItem[] = [];
  userGroupList: DdlItem[] = [];

  private getModal(): HTMLElement {
    return this.modal.nativeElement;
  }

  loadForm(obj: User): void {
    removeValidationErrors(this.formElement);
    hideShowModal(this.getModal(), 'show');
    this.obj = obj;
    this.api
      .loadDropDownList('App.Service.CompanyService', '0')
      .subscribe((resp) => {
        this.companyList = resp;
      });
    this.api
      .loadDropDownList('App.Service.UserGroupService', '0')
      .subscribe((resp) => {
        this.userGroupList = resp;
      });
    this.api
      .loadDropDownList('App.Service.UserTypeService', '0')
      .subscribe((resp) => {
        this.userTypeList = resp;
      });
  }

  onSubmit(form: NgForm): void {
    var isValid = validateForm(this.formElement);
    if (isValid) {
      this.obj.AccessActivities = null;
      this.obj.Company = null;
      this.obj.DemoTabs = null;
      this.obj.UserGroup = null;
      this.obj.UserSessions = null;
      this.obj.UserType = null;
      this.api.edit(this.obj).subscribe((resp) => {
        alert(resp.message);
        hideShowModal(this.getModal(), 'hide');
        this.shouldRefresh.emit(true);
      });
    }
  }
}
