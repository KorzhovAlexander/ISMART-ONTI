<template>
  <DxDropDownBox
      :ref="dropDownBoxRefName"
      :drop-down-options="dropDownOptions"
      :data-source="dataSourceServices"
      :value.sync="currentValue"
      :disabled="disabled"
      display-expr="name"
      value-expr="id"
      content-template="contentTemplate"
  >
    <template #contentTemplate="{}">
      <div>
        <DxDataGrid
            id="typeDropBox"
            :data-source="dataSourceServices"
            :height="400"
            :render-async="true"
            :remote-operations="true"
            :selected-row-keys="[currentValue]"
            :hover-state-enabled="true"
            :show-row-lines="true"
            :column-auto-width="true"
            :allow-column-reordering="true"
            :column-min-width="50"
            :allow-column-resizing="true"
            :word-wrap-enabled="true"
            :on-selection-changed="onSelectionChanged"
            :focused-row-enabled="true"
            :focused-row-key="currentValue"
            value-expr="id"
        >
          <DxColumn data-field="name" caption="Название"/>
          <DxColumn data-field="description" caption="Описание"/>
          <DxSearchPanel :visible="true" :width="450"/>
          <DxPaging :enabled="true" :page-size="20"/>
          <DxScrolling mode="virtual" row-rendering-mode="virtual" column-rendering-mode="virtual"/>
          <DxSelection mode="single"/>
        </DxDataGrid>
      </div>
    </template>
  </DxDropDownBox>
</template>

<script>
import * as AspNetData from "devextreme-aspnet-data-nojquery";
import {DxDataGrid, DxPaging, DxSelection, DxScrolling, DxColumn, DxSearchPanel} from 'devextreme-vue/data-grid'
import DxDropDownBox from 'devextreme-vue/drop-down-box'

const dropDownBoxRefName = 'dropDownBoxRef';
const dataSourceServices = AspNetData.createStore({
  key: 'id',
  loadUrl: `/api/ServiceType`,
  onBeforeSend: (method, ajaxOptions) => {
    ajaxOptions.xhrFields = {withCredentials: true};
  },
});
export default {
  name: "drop-down-service-select",

  components: {DxDataGrid, DxPaging, DxSelection, DxScrolling, DxColumn, DxDropDownBox, DxSearchPanel},

  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
    value: {
      type: Number,
      default: null
    },
    onValueChanged: {
      type: Function,
      default: () => function () {
      }
    },
  },


  data() {
    return {
      currentValue: this.value,
      dropDownOptions: {width: 500},
      dataSourceServices,
      dropDownBoxRefName,
    }
  },

  methods: {
    onSelectionChanged(selectionChangedArgs) {
      this.currentValue = selectionChangedArgs.selectedRowKeys[0];
      if (selectionChangedArgs.selectedRowKeys.length > 0) {
        this.onValueChanged(this.currentValue);
        this.$refs[dropDownBoxRefName].instance.close();
      }
    }
  }
}
</script>

<style scoped>

</style>