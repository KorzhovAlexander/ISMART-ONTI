<template>
  <DxPopup
      height="auto"
      :width="728"
      position="center"
      title="Форма поиска заказа"
      :show-title="true"
      :resize-enabled="true"
      :visible="visible"
      :close-on-outside-click="true"
      @hidden="cancel"
  >
    <div class="find-form">
      <DxTextBox
          v-model:value="currencyValue"
          styling-mode="filled"
          placeholder="Номер заказа"
          :show-clear-button="true"
      >
      </DxTextBox>
      <br>
      <DxButton
          :width="120"
          text="Найти"
          type="default"
          styling-mode="contained"
          @click="submit($event)"
      />
      <br>
      <br>

      <div v-if="orderInfo" style="text-align: center">
        <DxProgressBar
            v-if="orderInfo.maxProgress"
            id="progress-bar-status"
            :min="0"
            :max="orderInfo.maxProgress"
            :status-format="statusFormat"
            :value="orderInfo.progress"
            width="100%"
        />
        <div v-if="orderInfo.id">
          <p>Сервис - {{orderInfo.nameServiceType}}</p>
          <p>Откуда доставка - {{orderInfo.startPoint}}</p>
          <p>Куда доставка - {{orderInfo.endPoint}}</p>
          <p v-if="orderInfo.userId">Курьер - {{orderInfo.username}}</p>
        </div>
        <br>
        <span v-if="orderInfo.message">{{ orderInfo.message }}</span>
      </div>
    </div>
  </DxPopup>
</template>

<script>
import {DxPopup} from 'devextreme-vue/popup';
import {DxProgressBar} from 'devextreme-vue/progress-bar';
import {DxTextBox,} from 'devextreme-vue/text-box';
import notify from 'devextreme/ui/notify';
import DxButton from 'devextreme-vue/button';

export default {
  name: "order-find-form",
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
      currencyValue: null,
      orderInfo: null,
    }
  },
  methods: {
    statusFormat(v) {
      return `Статус Вашего заказа '${this.orderInfo.statusName}'`
    },
    cancel: function () {
      this.$emit('update:visible', false);
    },

    async submit() {
      if (!this.currencyValue) return;
      this.orderInfo = {message: 'Поиск начат...'};

      fetch(`api/order/track-order-${this.currencyValue}`, {
        method: "GET",
        credentials: 'include',
      })
          .then(res => {
            if (res.ok) return res.json()
            this.orderInfo = {message: 'Заказ не найден'};
          })
          .then(res => {
            console.log('get details', res)
            if (res)
              this.orderInfo = res;
          })
          .catch(c => {
            this.orderInfo = {message: 'Заказ не найден'};
            console.log(c)
          });
    },
  },
  components: {
    DxPopup,
    DxProgressBar,
    DxTextBox,
    DxButton,
  },
}
</script>

<style>
.find-form {
  align-self: center;
  justify-content: center;
  text-align: center;
}

#progress-bar-status {
  display: inline-block;
}

</style>