<template>
  <DxPopup
      height="auto"
      :width="728"
      position="center"
      title="Форма создания заказа"
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
              data-field="startPoint"
              editor-type="dxTextBox"
          />
          <DxSimpleItem
              :col-span="1"
              :label="{text: 'Куда доставляем'}"
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
                  :allow-updating="true"
                  :allow-deleting="true"
                  :allow-adding="true"
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
import {DxPopup} from "devextreme-vue/popup";
import {DxButtonItem, DxForm, DxGroupItem, DxSimpleItem} from "devextreme-vue/form";
import DxDataGrid, {DxEditing, DxColumn, DxValidationRule, DxLookup} from 'devextreme-vue/data-grid';
import * as AspNetData from "devextreme-aspnet-data-nojquery";
import DropDownServiceSelect from "./ui/drop-down-service-select";
import auth from "../auth";

const dataSourceUnits = AspNetData.createStore({
  key: 'id',
  loadUrl: `/api/unit`,
  onBeforeSend: (method, ajaxOptions) => {
    ajaxOptions.xhrFields = {withCredentials: true};
  },
});
export default {
  name: "order-create-form",
  props: {
    visible: {
      type: Boolean,
      required: true
    },
    formData: {
      type: Object,
      required: true
    },
  },
  watch: {
    visible: async function (isVisible) {
      if (isVisible) {
        this.localDataSource = []
      }
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

      localDataSource: [],
      dataSourceUnits,
    }
  },
  created: function () {
  },
  methods: {
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