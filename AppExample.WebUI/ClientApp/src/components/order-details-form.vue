<template>
  <DxPopup
      height="auto"
      :width="728"
      position="center"
      title="Форма управления заказом"
      :show-title="true"
      :resize-enabled="true"
      :visible="visible"
      :close-on-outside-click="true"
      @hidden="cancel"
  >
    <div>
      <DxForm
          :ref="formRefName"
          :form-data="formData"
          :col-count="2"
      >
        <DxGroupItem
            :col-count="2"
            :col-span="2"
            name="basicInfo"
        >
          <DxSimpleItem
              :col-span="1"
              :label="{text: 'Откуда доставляем'}"
              :editorOptions="{disabled:!isManager}"
              data-field="startPoint"
              editor-type="dxTextBox"
          />
          <DxSimpleItem
              :col-span="1"
              :label="{text: 'Куда доставляем'}"
              :editorOptions="{disabled:!isManager}"
              data-field="endPoint"
              editor-type="dxTextBox"
          />

          <DxSimpleItem
              :col-span="2"
              :label="{text: 'Тип сервиса'}"
              data-field="serviceTypeId"
              template="dropDownServiceEditor"
          />
        </DxGroupItem>
        <template #dropDownServiceEditor="{ data }">
          <DropDownServiceSelect
              :value="formData[data.dataField]"
              :disabled="!isManager"
              :on-value-changed="changeService"
          />
        </template>


        <DxGroupItem
            :col-count="2"
            :col-span="2"
            caption=""
            name="structure"
        >
          <template #default>
            <DxDataGrid
                :data-source="localDataSource"
                key-field="key"
            >
              <DxColumn
                  data-field="product"
                  caption="Продукт"
              >
                <DxValidationRule
                    type="required"
                />
              </DxColumn>
              <DxColumn
                  data-field="unitId"
                  caption="Единица измерения"
              >
                <DxValidationRule
                    type="required"
                />
                <DxLookup
                    :data-source="dataSourceUnits"
                    value-expr="id"
                    display-expr="fullName"
                />
              </DxColumn>
              <DxColumn
                  data-field="count"
                  data-type="number"
                  caption="Колличество"
              >
                <DxValidationRule
                    type="required"
                />
              </DxColumn>

              <DxEditing
                  :allow-updating="isManager"
                  :allow-deleting="isManager"
                  :allow-adding="isManager"
                  mode="cell"
              />
            </DxDataGrid>
          </template>
        </DxGroupItem>

        <DxGroupItem
            :col-count="2"
            :col-span="2"
            vertical-alignment="bottom"
        >
          <DxButtonItem
              v-on:keyup.enter="submit"
              :button-options="{text: 'Сохранить',icon:'check', type: 'default',onClick: submit}"
              horizontal-alignment="right"
          />
          <DxButtonItem
              v-on:keyup.esc="cancel"
              :button-options="{text: 'Отменить',icon:'close',type: 'danger',styling:'outline',onClick: cancel}"
              horizontal-alignment="left"
          />
        </DxGroupItem>
      </DxForm>
    </div>
  </DxPopup>
</template>

<script>
import notify from 'devextreme/ui/notify'
import auth from "../auth";
import {DxPopup} from "devextreme-vue/popup";
import {DxButtonItem, DxForm, DxGroupItem, DxSimpleItem} from "devextreme-vue/form";
import DxDataGrid, {DxEditing, DxColumn, DxValidationRule, DxLookup} from 'devextreme-vue/data-grid';
import * as AspNetData from "devextreme-aspnet-data-nojquery";
import DropDownServiceSelect from "./ui/drop-down-service-select";

const dataSourceUnits = AspNetData.createStore({
  key: 'id',
  loadUrl: `/api/unit`,
  onBeforeSend: (method, ajaxOptions) => {
    ajaxOptions.xhrFields = {withCredentials: true};
  },
});
export default {
  name: "order-details-form",
  props: {
    visible: {
      type: Boolean,
      required: true
    },
    orderId: {
      type: Number,
      required: true
    },
  },
  computed: {
    form: function () {
      return this.$refs[this.formRefName].instance;
    },
  },

  data: function () {
    return {
      popupVisible: false,
      formRefName: 'form',
      isManager: false,

      formData: {},
      localDataSource: [],
      dataSourceUnits,
    }
  },
  watch: {
    visible: async function (isVisible) {
      if (isVisible) {
        this.isManager = auth.hasManagerRole()
        await this.initOrderInformation()
      }
    },
  },
  created: function () {
  },
  methods: {
    async initOrderInformation() {
      return await fetch(`api/order/details/${this.orderId}`, {
        method: "GET",
        credentials: 'include',
      })
          .then(res => res.json())
          .then(res => {
            console.log('get details', res)
            this.formData = res;
            this.localDataSource = res.orderStructures;
            return {
              isOk: true,
              data: this.formData
            };
          })
          .catch(c => {
            console.log(c)
            return {
              isOk: false
            };
          });
    },
    changeService: function (serviceId) {
      this.formData.serviceTypeId = serviceId;
    },

    cancel: function () {
      this.$emit('update:visible', false);
    },

    submit: function () {
      this.formData.orderStructures = this.localDataSource;

      const validateResult = this.form.validate();

      if (validateResult.isValid) {
        this.$emit('submit', this.formData);
      }
    },
  },
  components: {
    DropDownServiceSelect,
    DxPopup,
    DxForm,
    DxSimpleItem,
    DxGroupItem,
    DxButtonItem,
    DxDataGrid,
    DxEditing,
    DxColumn,
    DxValidationRule,
    DxLookup
  },
}
</script>

<style scoped>

</style>