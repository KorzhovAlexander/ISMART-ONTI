<template>
  <div>
    Email подтвержден
  </div>
</template>

<script>
import DxTextBox from "devextreme-vue/text-box";
import DxButton from "devextreme-vue/button";
import axios from "axios";
import notify from "devextreme/ui/notify";

export default {
  name: "confirm-email",
  data() {
    return {}
  },
  methods: {},
  created() {
    let data = this.$router.currentRoute.query;
    axios.post(`api/account/confirm-email?userId=${data.userId}&code=${data.code}`)
        .then((response) => {
          notify('Email подтвержден', 'success', 3000)
        })
        .catch((error) => {
          notify('Ошибка подтверждения. Ссылка не действиетельна', 'Error', 3000)
        })
  },
  components: {
    DxTextBox,
    DxButton
  }
}
</script>

<style>

</style>