<template>
    <b-card no-body class="task" :data-id="task.id">
        <b-card-header class="row align-items-start m-0" header-tag="header" role="tab">
            <!-- Display data --> 
            <b-card-text class="col-1"><b-icon class="drag-handle" icon="grip-vertical"></b-icon></b-card-text>
            <b-card-text class="col" v-b-toggle="'task-accordion-'+task.id">{{ task.summary }}</b-card-text>
            <b-card-text class="col">{{ task.createdAt }}</b-card-text>
            <b-card-text class="col">{{ task.priority }}</b-card-text>
        </b-card-header>
        <b-collapse :id="'task-accordion-'+task.id" role="tabpanel" @show="onShow">
            <b-card-body>
                <task-editor :task="task" :autosave="true" @save="save"></task-editor>
                <!-- Subtasks --> 
                <div class="sub-task-container align-middle">
                    <!-- No tasks disclaimer --> 
                    <span class="no-items" v-if="!task.children || task.children.length === 0">No tasks</span>
                    <!-- Task loading spinner -->
                    <div class="text-center" v-if="loadingChildren">
                        <b-spinner label="Loading"></b-spinner>
                    </div>

                    <draggable class="sub-tasks" :list="task.children" group="tasks" handle=".drag-handle" ghost-class="ghost">                    
                        <task v-for="child in task.children" :key="child.id" v-bind:task="child" :parentId="task.id" @save="save" @remove="remove" />
                    </draggable>
                </div>
                <b-button v-b-modal="'new-task-' + task.id">New sub-task</b-button>
                <b-button @click="remove(null)" type="danger">Delete</b-button>
            </b-card-body>
        </b-collapse>
        <task-editor :id="'new-task-' + task.id" :autosave="false" :ismodal="true" @save="save"></task-editor>
    </b-card>
</template>
<script lang="js">
import Vue from 'vue';
import draggable from 'vuedraggable';
import taskEditor from './TaskEditor.vue';
import axios from 'axios';

export default Vue.extend({
    name: 'task',
    components: {
        draggable,
        taskEditor
    },
    data() {
        return {
            loadingChildren: false,
            config: {
                validation: {
                    maxSummary: 128,
                    maxDescription: 1024
                },
                taskStates: [
                    { value: 0, text: '' },
                    { value: 1, text: 'Reserved' },
                    { value: 2, text: 'Ongoing' },
                    { value: 3, text: 'Done' },
                    { value: 4, text: 'Pending' }
                ]
            }
        }
    },
    props: {
        task: Object,
        tasks: Array,
        parentId: { type: Number, default: null }
    },
    computed: {
        summaryState() {
            if ( !this.task.summary ) return null;
            if ( this.task.summary.length > this.config.validation.maxSummary ) return false;
            return true;
        },
        descriptionState() {
            if ( !this.task.description ) return null;
            if ( this.task.description.length > this.config.validation.maxDescription ) return false;
            return true;
        },
        deadlineState() {
            if ( !this.task.deadlineAt ) return null;
            if ( isNaN(Date.parse(this.task.deadlineAt)) ) return false;
            return true;
        },
        priorityState() {
            if ( typeof this.task.priority !== "number" ) return false;
            if ( !this.task.priority ) return null;
            if ( !Number.isInteger(this.task.priority) ) return false;
            if ( !Number.isSafeInteger(this.task.priority) ) return false;
            return true;
        }
    },
    created: function() {
        // this ensure state consistency with parent id
        if ( this.task.parentId !== this.parentId ) {
            this.task.parentId = this.parentId;    
            this.save(this.task);
        }
    },
    methods: {
        // Input change
        save(task, onSavedCallback) {
            if (!task.parentId && task.id != this.task.id) {
                task.parentId = this.task.id;
            }
            if (!onSavedCallback) {
                onSavedCallback = this.onSaved;
            }

            this.$emit("save", task, onSavedCallback);
        },
        onSaved(task) {
            if (!this.task.children) {
                this.task.children = [];
            }

            this.task.children.push(task);
        },
        remove(task, onRemovedCallback) {
            if (!onRemovedCallback && task) {
                onRemovedCallback = this.onRemoved;
            }
            if ( !task ) {
                task = this.task;
            }
            this.$emit("remove", task, onRemovedCallback );
        },
        onRemoved(data) {
           
            let i = this.task.children.findIndex(c => c.id == data.id);

            this.$delete(this.task.children, i);
        },
        onShow(){
            // Lazy load children when drilling down
            if ( !this.task.children ) {
                this.loadingChildren = true;
                axios.get(`/api/tasks/${this.task.id}`).then(r => {
                    this.task.children = r.data.children;
                }).finally(() => 
                {
                    this.loadingChildren = false;
                })   
            }
        }
    }
});
</script>
<style scoped>
    .sub-task-container {
        background: #eee;
    }
    span.no-items {
        position: relative;
        top: 50%;
        transform: translateY(-50%);
    }

    .sub-tasks {
        min-height: 50px;
    }
</style>