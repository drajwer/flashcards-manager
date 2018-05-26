import React from 'react';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import * as Ons from 'react-onsenui';

import BasePageWithMenu from './base/BasePageWithMenu';
import CategoriesListView from '../components/CategoriesListView';
import ErrorView from '../components/ErrorView';
import FetchingView from '../components/FetchingView';

import * as Actions from '../actions';
import { resetState } from '../actions';



class SelectCategoryPage extends BasePageWithMenu {
  componentDidMount() {
    this.props.actions.getCategories();
  }
  resetState() {
    this.props.resetState();
  }
  handleCategorySelect = (categoryId) => {
    const { navigator, actions } = this.props;
    navigator.pushPage({ key: 'SELECT_MODE_PAGE' }, { animation: 'slide' });
    actions.selectCategory(categoryId);
  }
  renderPage() {
    const { categories, isFetching, isInvalid } = this.props.learning;
    if (isInvalid) {
      return <ErrorView />;
    };
    return (
      <div>
        {
          isFetching
            ? <FetchingView />
            : <CategoriesListView
              title="Wszystkie kategorie"
              onCategorySelect={this.handleCategorySelect}
              categories={categories} />
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
  learning: state.learning
});

export default connect(mapStateToProps, mapDispatchToProps)(SelectCategoryPage);