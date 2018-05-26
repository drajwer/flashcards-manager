import React from 'react';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import * as Ons from 'react-onsenui';

import CategoryView from '../components/CategoryView';
import ErrorView from '../components/ErrorView';
import FetchingView from '../components/FetchingView';

import * as Actions from '../actions/manageCategories';
import BasePageWithBackButton from './base/BasePageWithBackButton';



class ManageCategoryPage extends BasePageWithBackButton {
    handleFlashcardsSelect = () => {
        const { navigator } = this.props;
        navigator.pushPage({key: 'MANAGE_FLASHCARDS_MAIN_PAGE'});
    }

    handleDeleteClick = () => {
        const { deleteCategory } = this.props.actions;
        const { categoryId } = this.props.manageCategories;
        const { navigator } = this.props;
        deleteCategory(categoryId);
        navigator.popPage();
    }

    handlePublicClick = () => {
        const { publishCategory } = this.props.actions;
        const { categoryId } = this.props.manageCategories;
        publishCategory(categoryId);
    }

    handleUpdateName = (name) => {
        const { updateNameCategory } = this.props.actions;
        const { categoryId } = this.props.manageCategories;
        updateNameCategory(categoryId, name);
    }

    handleDismissClick = () => {
        const { resetToast } = this.props.actions;
        resetToast();
    }

    renderPage() {
        const { categories, isFetching, isInvalid, categoryId, showToast, message } = this.props.manageCategories;
        const category = categories.find(c => c.id === categoryId);
        if (isInvalid) {
            return <ErrorView />;
        };
        return (
            <div>
                {
                    (isFetching || category == null)
                        ? <FetchingView />

                        : <CategoryView
                            category={category}
                            onDeleteClick={this.handleDeleteClick}
                            onPublicClick={this.handlePublicClick}
                            onUpdateName={this.handleUpdateName}
                            onFlashcardsClick={this.handleFlashcardsSelect}
                            showToast={showToast}
                            message={message}
                            onDismissClick={this.handleDismissClick}
                        />
                }
            </div>
        )
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(Actions, dispatch)
    };
};

const mapStateToProps = (state) => ({
    manageCategories: state.manageCategories
});

export default connect(mapStateToProps, mapDispatchToProps)(ManageCategoryPage);