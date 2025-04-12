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
import { UserGroup, emptyUserGroup } from '../page-entities/usergroup.entity';
import { DdlItem } from '../page-entities/ddl-item.entity';
import { UserGroupService } from './usergroup.service';
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
              <i class="fa fa-edit"></i> Edit User Group
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
                <input [(ngModel)]="obj.UserGroupId" type="hidden" name="UserGroupId" />
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
  @Input() obj: UserGroup = emptyUserGroup();

  api = inject(UserGroupService);
  companyList: DdlItem[] = [];

  private getModal(): HTMLElement {
    return this.modal.nativeElement;
  }

  loadForm(obj: UserGroup): void {
    removeValidationErrors(this.formElement);
    hideShowModal(this.getModal(), 'show');
    this.obj = obj;
  }

  onSubmit(form: NgForm): void {
    var isValid = validateForm(this.formElement);
    if (isValid) {
      this.api.edit(this.obj).subscribe((resp) => {
        alert(resp.message);
        hideShowModal(this.getModal(), 'hide');
        this.shouldRefresh.emit(true);
      });
    }
  }
}
