import React from 'react';

import * as Ons from 'react-onsenui';

import NavBar from '../../components/NavBar';
import LocationList from '../../containers/LocationList';
import AddLocation from '../../containers/AddLocation';
import { routeLabels } from '../../routing/routeLabels';

export default class BasePageWithMenu extends React.Component {
  constructor() {
    super();
    this.state = { isOpen: false };
  }
  hide = () => this.setState({ isOpen: false });
  show = () => this.setState({ isOpen: true });
  handleLinkClick(newKey) {
    const { navigator, pageKey } = this.props;
    this.hide();
    this.resetState();
    if (pageKey !== newKey) {
      navigator.resetPage({ key: newKey }, { animation: 'fade' });
    }
  }
  render() {
    const { navigator, title } = this.props;
    return (
      <Ons.Page>
        <Ons.Splitter>
          <Ons.SplitterSide
            style={{
              boxShadow: '0 10px 20px rgba(0,0,0,0.19), 0 6px 6px rgba(0,0,0,0.23)'
            }}
            side='left'
            width={200}
            collapse={true}
            swipeable={true}
            isOpen={this.state.isOpen}
            onClose={this.hide}
            onOpen={this.show}
          >
            <Ons.Page>
              <div style={{margin: '10px 10px 10px 10px', fontWeight: 'bold', fontSize: '22px'}}>Fishki</div>
              <Ons.List
                dataSource={Object.keys(routeLabels)}
                renderRow={(key) => {
                  const obj = routeLabels[key];
                  return (
                    obj.showInMenu &&
                    <Ons.ListItem
                      key={key}
                      onClick={() => this.handleLinkClick(key)}
                      tappable>
                      <div className='left'>
                        {obj.icon && <Ons.Icon icon={obj.icon} />}
                      </div>
                      <div className='center'>
                        {obj.label}
                      </div>
                    </Ons.ListItem>
                  );
                }
                }
              />
            </Ons.Page>
          </Ons.SplitterSide>
          <Ons.SplitterContent>
            <Ons.Page renderToolbar={() => <NavBar title={title} navigator={navigator} onButtonClick={this.show} />}>
              {this.renderPage()}
            </Ons.Page>
          </Ons.SplitterContent>
        </Ons.Splitter>
      </Ons.Page>
    );
  }
}
