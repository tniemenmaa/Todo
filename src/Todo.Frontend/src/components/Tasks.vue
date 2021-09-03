<template>
    <template v-for="task in tasks">

    </template>
</template>
<script lang="js">
    import Vue from 'vue';
    import axios from 'axios';

    export default Vue.extend({
        data() {
            return {
                loading: false,
                tasks: []
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;
                axios.get("/api/tasks").then(d => {
                    console.log(d.data);
                })
            }
        }
    });
</script>