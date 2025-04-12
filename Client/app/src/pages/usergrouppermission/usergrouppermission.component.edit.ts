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
import {
  UserGroupPermission,
  emptyUserGroupPermission,
} from '../page-entities/usergrouppermission.entity';
import { DdlItem } from '../page-entities/ddl-item.entity';
import { UserGroupPermissionService } from './usergrouppermission.service';
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
              <i class="fa fa-edit"></i> Edit UserGroupPermission
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
                <input [(ngModel)]="obj.UserGroupPermissionId" type="hidden" name="UserGroupPermissionId" />
                <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >Menu Category
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="MenuCategoryId"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <select
                      [(ngModel)]="obj.MenuCategoryId"
                      name="MenuCategoryId"
                      class="form-control"
                    >
                      <option value="0">--SELECT--</option>
                      <option
                        value="{{ item.Value }}"
                        *ngFor="let item of menuCategoryList"
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
                      [(ngModel)]="obj.UserGroupId"
                      name="UserGroupId"
                      class="form-control"
                    >
                      <option value="0">--SELECT--</option>
                      <option
                        value="{{ item.Value }}"
                        *ngFor="let item of userGroupList"
                      >
                        {{ item.Text }}
                      </option>
                    </select>
                  </div>
                </div>
                <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >Webpage
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="WebpageId"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <select
                      [(ngModel)]="obj.WebpageId"
                      name="WebpageId"
                      class="form-control"
                    >
                      <option value="0">--SELECT--</option>
                      <option
                        value="{{ item.Value }}"
                        *ngFor="let item of webPageList"
                      >
                        {{ item.Text }}
                      </option>
                    </select>
                  </div>
                </div>
                <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >Is Visible? &nbsp;&nbsp;&nbsp;
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="IsVisible"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <input
                      [(ngModel)]="obj.IsVisible"
                      type="checkbox"
                      name="IsVisible"
                      placeholder="IsVisible"
                      data-val="true"
                      data-val-required="The IsVisible field is required."
                      autocomplete="off"
                    />
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
  @Input() obj: UserGroupPermission = emptyUserGroupPermission();

  api = inject(UserGroupPermissionService);
  menuCategoryList: DdlItem[] = [];
  userGroupList: DdlItem[] = [];
  webPageList: DdlItem[] = [];

  private getModal(): HTMLElement {
    return this.modal.nativeElement;
  }

  loadForm(obj: UserGroupPermission): void {
    removeValidationErrors(this.formElement);
    hideShowModal(this.getModal(), 'show');
    this.obj = obj;
    this.api
      .loadDropDownList('App.Service.MenuCategoryService', '0')
      .subscribe((resp) => {
        this.menuCategoryList = resp;
      });
    this.api
      .loadDropDownList('App.Service.UserGroupService', '0')
      .subscribe((resp) => {
        this.userGroupList = resp;
      });
    this.api
      .loadDropDownList('App.Service.WebpageService', '0')
      .subscribe((resp) => {
        this.webPageList = resp;
      });
  }

  onSubmit(form: NgForm): void {
    var isValid = validateForm(this.formElement);
    if (isValid) {
      this.obj.MenuCategory = null;
      this.obj.UserGroup = null;
      this.obj.Webpage = null;
      this.api.edit(this.obj).subscribe((resp) => {
        alert(resp.message);
        hideShowModal(this.getModal(), 'hide');
        this.shouldRefresh.emit(true);
      });
    }
  }
}
