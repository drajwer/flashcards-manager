import React from 'react';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import * as Ons from 'react-onsenui';
import ons from 'onsenui';

import BasePageWithMenu from './base/BasePageWithMenu';
import CategoriesListView from '../components/CategoriesListView';
import ErrorView from '../components/ErrorView';
import FetchingView from '../components/FetchingView';

import * as Actions from '../actions/manageCategories';
import { resetState } from '../actions/index';



class ManageCategoriesMainPage extends BasePageWithMenu {
  componentDidMount() {
    const { categories, isInvalid } = this.props.manageCategories;
    if (!isInvalid && categories.length > 0) {
      return;
    }
    this.props.actions.getOwnCategories();
  }
  resetState() {
    this.props.resetState();
  }
  handleCategorySelect = (categoryId) => {
    const { navigator, actions, categories } = this.props;

    navigator.pushPage({ key: 'MANAGE_CATEGORY_PAGE' }, { animation: 'slide' });
    actions.selectCategory(categoryId);
  }
  handleAddClick = (categoryName) => {
    const { addCategory } = this.props.actions;
    addCategory(categoryName);
  }

  handleDismissClick = () => {
    const { resetToast } = this.props.actions;
    resetToast();
  }
  renderPage() {
    const { categories, isFetching, isInvalid, showToast, message } = this.props.manageCategories;
    if (isInvalid) {
      return <ErrorView />;
    };
    return (
      <div>
        {
          isFetching
            ? <FetchingView />

            : <CategoriesListView
              onAddButtonClick={() => alert('add')}
              title="Twoje kategorie"
              onCategorySelect={this.handleCategorySelect}
              categories={categories}
              showToast={showToast}
              message={message}
              onDismissClick={this.handleDismissClick}
              onAddButtonClick={this.handleAddClick}
            />
        }
      </div>
    )
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(Actions, dispatch),
    resetState: bindActionCreators(resetState, dispatch)
  };
};

const mapStateToProps = (state) => ({
  manageCategories: state.manageCategories
});

export default connect(mapStateToProps, mapDispatchToProps)(ManageCategoriesMainPage);