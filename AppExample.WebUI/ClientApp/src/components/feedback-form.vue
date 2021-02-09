<template>
  <DxPopup
      height="auto"
      :width="728"
      position="center"
      title="Форма обратной связи"
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
              :col-span="2"
              :label="{text: 'Сообщение'}"
              data-field="message"
              editor-type="dxTextArea"
          />
          <DxSimpleItem
              :col-span="2"
              :label="{text: 'Email для связи'}"
              data-field="email"
              editor-type="dxTextBox"
          />
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

export default {
  name: "feedback-form",
  props: {
    visible: {
      type: Boolean,
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

      localDataSource: [],
      formData: {
        message: '',
        email: ''
      },
    }
  },
  created: function () {
  },
  methods: {
    cancel: function () {
      this.$emit('update:visible', false);
    },

    submit: function () {
      const validateResult = this.form.validate();

      if (validateResult.isValid) {
        this.$emit('submit', this.formData);
      }
    },
  },
  components: {
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