<template>
  <DxPopup
      height="auto"
      :width="728"
      position="center"
      title="Виды услуг"
      :show-title="true"
      :resize-enabled="true"
      :visible="visible"
      :close-on-outside-click="true"
      @hidden="cancel"
  >
    <div>
      <DxDataGrid
          :data-source="dataSource"
      >
        <DxColumn
            data-field="name"
            caption="Наименовнаие"
        />
        <DxColumn
            data-field="description"
            caption="Описание"
        />
        <DxEditing
            :allow-updating="true"
            :allow-deleting="true"
            :allow-adding="true"
            mode="row"
            :use-icons="true"
        />
      </DxDataGrid>
    </div>
  </DxPopup>
</template>

<script>
import {DxPopup} from "devextreme-vue/popup";
import DxDataGrid, {DxColumn, DxEditing, DxLookup, DxValidationRule} from "devextreme-vue/data-grid";
import * as AspNetData from "devextreme-aspnet-data-nojquery";

const dataSource = AspNetData.createStore(
    {
      key: 'id',
      loadUrl: `/api/servicetype`,
      insertUrl: `/api/servicetype`,
      updateUrl: `/api/servicetype`,
      deleteUrl: `/api/servicetype`,
      onBeforeSend: (method, ajaxOptions) => {
        ajaxOptions.xhrFields = {withCredentials: true};
        ajaxOptions.contentType = 'application/json';

        switch (method) {
          case 'insert': {
            ajaxOptions.data = ajaxOptions.data.values;
            break;
          }
          case 'update': {
            let data = JSON.parse(ajaxOptions.data.values);
            data['id'] = ajaxOptions.data.key;
            ajaxOptions.url += `/${ajaxOptions.data.key}`;
            ajaxOptions.data = JSON.stringify(data);
            break;
          }
          case 'delete': {
            ajaxOptions.url += `/${ajaxOptions.data.key}`;
            break;
          }
        }
      },
    }
)

export default {
  name: "service-type-form",
  props: {
    visible: {
      type: Boolean,
      required: true
    },
  },
  data: function () {
    return {
      popupVisible: false,
      formRefName: 'form',

      dataSource,
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