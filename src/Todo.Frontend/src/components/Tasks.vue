<template>
    <div>
        <div class="row justify-content-center m-0 mb-3">
            <b-form-input class="col-8" v-model="search" placeholder="Search tasks"></b-form-input>
            <b-form-select class="col-2 ml-2" v-model="sorting" :options="config.sortingOptions"></b-form-select>
        </div>
        <div class="row m-0 p-3 headers">
            <div class="col-1"></div>
            <div class="col pl-0 text-left">Summary</div>
            <div class="col d-none d-lg-block">Created</div>
            <div class="col">Deadline</div>
            <div class="col">Priority</div>
            <div class="col">State</div>
        </div>
        <div v-if="loading" class="m-3">
            <p class="text-muted font-weight-bold">Loading tasks...</p>
            <b-spinner label="Loading tasks..."></b-spinner>
        </div>
        <!-- Render all the base level tasks -->
        <draggable :list="computedTasks" :group="{ name: 'tasks' }" handle=".drag-handle" ghost-class="ghost">
            <task v-for="task in computedTasks" :key="task.id" :task="task" @save="save" @remove="remove" />
        </draggable>
        <div class="row justify-content-end m-0 mt-3">
            <div class="col text-right">
                <b-button pill variant="primary" v-b-modal.new-task>New Task</b-button>
            </div>
        </div>
        <task-editor id="new-task" :autosave="false" :ismodal="true" @save="save" title="New Task" ></task-editor>
    </div>
</template>
<script lang="js">
    import Vue from 'vue';
    import axios from 'axios';
    import task from './Task.vue';
    import taskEditor from './TaskEditor.vue';
    import draggable from 'vuedraggable';
    import sorting from '../config/sorting.js'

    export default Vue.extend({
        components: {
            task,
            draggable,
            taskEditor
        },
        data() {
            return {
                config: {
                    sortingOptions: sorting.options
                },
                loading: false,
                tasks: [],
                sorting: null,
                search: ''
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        computed: {
            computedTasks() {
                let tasks = this.tasks;
                if (this.sorting) {
                    tasks.sort((a,b) => { 
                        if (a[this.sorting.field] > b[this.sorting.field]) {
                            return this.sorting.ascending ? 1 : -1;
                        }
                        if (a[this.sorting.field] < b[this.sorting.field]) {
                            return this.sorting.ascending ? -1 : 1;
                        }
                        return 0;
                     });    
                }
                if (this.search) {
                    tasks = tasks.filter(t => t.summary.toLowerCase().includes(this.search.toLowerCase()));
                }
                return tasks;
            }
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;
                axios.get(`${process.env.VUE_APP_API_BASE_URL}/api/tasks`).then(r => {
                    this.tasks = r.data;
                })
                .catch((err) => {
                    this.$bvToast.toast(err.details ? err.details : 'Unknown error while loading tasks.', 
                    {
                        title: 'Error',
                        autoHideDelay: 5000,
                        appendToast: true,
                        solid: true,
                        variant: 'danger',
                        toaster: 'b-toaster-top-center'
                    })
                })
                .finally(() => {
                    this.loading = false;
                })
            },
            save(task, savedCallback) { 
                // make a copy of the data and save changes
                let data = JSON.parse(JSON.stringify(task));
                data.children = null;
                
                if ( data.id ) {
                    axios.put(`${process.env.VUE_APP_API_BASE_URL}/api/tasks/${data.id}`, data)                
                        .catch((err) => {
                            this.$bvToast.toast(err.details ? err.details : 'Unknown error while saving task.', {
                                title: 'Error',
                                autoHideDelay: 5000,
                                appendToast: true,
                                solid: true,
                                variant: 'danger',
                                toaster: 'b-toaster-top-center'
                        })
                    });
                }
                else {
                    axios.post(`${process.env.VUE_APP_API_BASE_URL}/api/tasks`, data).then(r => {
                        if (savedCallback) {
                            savedCallback(r.data);
                        }
                        else {
                            this.tasks.push(r.data);
                        }
                    })
                    .catch((err) => {
                            this.$bvToast.toast(err.details ? err.details : 'Unknown error while saving task.', {
                                title: 'Error',
                                autoHideDelay: 5000,
                                appendToast: true,
                                solid: true,
                                variant: 'danger',
                                toaster: 'b-toaster-top-center',
                        });
                    })
                }
            },
            remove(task, removedCallback) {
                axios.delete(`${process.env.VUE_APP_API_BASE_URL}/api/tasks/${task.id}`).then(() => {
                    if (removedCallback) {
                        removedCallback(task);
                    }
                    else {
                        let i = this.tasks.findIndex(c => c.id == task.id);
                        this.$delete(this.tasks, i);
                    }
                });
            }
        },
    });
</script>