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
  MenuCategory,
  emptyMenuCategory,
} from '../page-entities/menucategory.entity';
import { DdlItem } from '../page-entities/ddl-item.entity';
import { MenuCategoryService } from './menucategory.service';
import {
  jQ,
  hideShowModal,
  validateForm,
  removeValidationErrors,
} from './../../shared/jquery-utils';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'add-modal',
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
              <i class="fa fa-plus"></i> Add New Menu Category
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
            #neoAddForm="ngForm"
            (submit)="onSubmit(neoAddForm)"
            class="box neo-add-form"
            novalidate
          >
            <div class="modal-body">
              <div class="row edit-form-field-container">
                <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >App Icon
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="AppIconId"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <select
                      #appIcon
                      [(ngModel)]="obj.AppIconId"
                      name="AppIconId"
                      class="form-control"
                      data-val="true"
                      data-val-range="*required."
                      data-val-required="*required."
                      data-val-range-min="1"
                      data-val-range-max="999999"
                    >
                      <option value="0" >
                        --SELECT--
                      </option>
                      <option
                        [attr.data-html]="item.Data"
                        value="{{ item.Value }}"
                        *ngFor="let item of iconList"
                      >
                        {{ item.Text }}
                      </option>
                    </select>
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
                      >Description
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="Description"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <input
                      [(ngModel)]="obj.Description"
                      type="text"
                      name="Description"
                      placeholder="Description"
                      class="form-control"
                    />
                  </div>
                </div>

                <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >Menu Order
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="MenuOrder"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <input
                      [(ngModel)]="obj.MenuOrder"
                      type="text"
                      name="MenuOrder"
                      placeholder="MenuOrder"
                      class="form-control"
                      data-val="true"
                      data-val-required="*required."
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
export class AddComponent {
  @ViewChild('neoAddForm', { read: ElementRef }) formElement!: ElementRef;
  @ViewChild('modal', { static: false }) modal!: ElementRef;
  @Output() shouldRefresh = new EventEmitter<boolean>();
  @Input() obj: MenuCategory = emptyMenuCategory();

  api = inject(MenuCategoryService);
  iconList: DdlItem[] = [];

  private getModal(): HTMLElement {
    return this.modal.nativeElement;
  }

  loadForm(): void {
    removeValidationErrors(this.formElement);
    this.obj = emptyMenuCategory();
    hideShowModal(this.getModal(), 'show');
    this.api
      .loadDropDownList('App.Service.AppIconService', '0')
      .subscribe((resp) => {
        this.iconList = resp;
      });
  }

  onSubmit(form: NgForm): void {
    var isValid = validateForm(this.formElement);
    if (isValid) {
      this.obj.MenuCategoryId = 0;
      this.obj.AppIcon = null;
      this.obj.UserGroupPermissions = null;
      this.api.add(this.obj).subscribe((resp) => {
        alert(resp.message);
        hideShowModal(this.getModal(), 'hide');
        this.shouldRefresh.emit(true);
      });
    }
  }
}
