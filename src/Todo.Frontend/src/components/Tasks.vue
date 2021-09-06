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
        <!-- Render all the base level tasks -->
        <draggable :list="computedTasks" :group="{ name: 'tasks' }" handle=".drag-handle" ghost-class="ghost">
            <task v-for="task in computedTasks" :key="task.id" :task="task" @save="save" @remove="remove" />
        </draggable>
        <div class="row justify-content-end m-0 mt-3">
            <div class="col text-right">
                <b-button pill variant="primary" v-b-modal.new-task>New Task</b-button>
            </div>
        </div>
        <task-editor id="new-task" :autosave="false" :ismodal="true" @save="save"></task-editor>
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
                axios.get('/api/tasks').then(r => {
                    this.tasks = r.data;
                })
            },
            save(task, savedCallback) { 
                // make a copy of the data and save changes
                let data = JSON.parse(JSON.stringify(task));
                data.children = null;
                
                if ( data.id ) {
                    axios.put(`/api/tasks/${data.id}`, data).then(r => {
                        console.log(r);
                    });
                }
                else {
                    axios.post('/api/tasks', data).then(r => {
                        if (savedCallback) {
                            savedCallback(r.data);
                        }
                        else {
                            this.tasks.push(r.data);
                        }
                    });
                }
            },
            remove(task, removedCallback) {
                axios.delete(`/api/tasks/${task.id}`).then(() => {
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